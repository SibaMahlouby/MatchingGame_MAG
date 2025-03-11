using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

/*
 This script is responsible for generating and managing the game board.
 It handles tile placement, user selection, match detection, and animations.
 */
public class BoardManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Transform boardParent;
    [SerializeField] private TileTypeAsset[] tileTypes;
    [SerializeField] private AudioClip matchSound;
    [SerializeField] private AudioSource audioSource;

    [Header("Settings")]
    [SerializeField] private float tweenDuration = 0.3f;
    [SerializeField] private int width = 5;
    [SerializeField] private int height = 5;

    private bool _isMatching;
    private List<Row> rows = new List<Row>();

    public event Action<TileTypeAsset, int> OnMatch;

    private void Start()
    {
        Debug.Log("Initializing BoardManager");
        GenerateBoard(width, height);
    }

    // Generates a new board of the given width and height.
    public void GenerateBoard(int width, int height)
    {
        Debug.Log("Generating board with dimensions: " + width + "x" + height);
        ClearBoard();

        for (int y = 0; y < height; y++)
        {
            GameObject rowObj = Instantiate(rowPrefab, boardParent);
            Row row = rowObj.GetComponent<Row>();
            row.InitializeRow(width, tilePrefab, rowObj.transform);
            rows.Add(row);
        }

        AssignTileTypes();
    }

    //Clears the existing board before creating a new one.
    private void ClearBoard()
    {
        Debug.Log("Clearing existing board");
        foreach (var row in rows)
        {
            Destroy(row.gameObject);
        }
        rows.Clear();
    }

    // Assigns random tile types to the board.
    private void AssignTileTypes()
    {
        Debug.Log("Assigning tile types");
        for (int y = 0; y < rows.Count; y++)
        {
            var row = rows[y];
            for (int x = 0; x < row.tiles.Length; x++)
            {
                var tile = row.tiles[x];
                tile.x = x;
                tile.y = y;
                tile.Type = tileTypes[UnityEngine.Random.Range(0, tileTypes.Length)];
                tile.button.onClick.AddListener(() => Select(tile));
            }
        }
    }

    // Handles tile selection and match detection.
    private async void Select(Tile tile)
    {
        if (_isMatching || tile == null) return;
        List<TileData> matchingTiles = new List<TileData>();
        FindMatchingTiles(tile, tile.Type, matchingTiles);

        if (matchingTiles.Count > 1)
        {
            Debug.Log("Match found with " + matchingTiles.Count + " tiles.");
            await HandleMatchAsync(new MatchTiles(tile.Data, matchingTiles.ToArray(), new TileData[0]));
        }
        else
        {
            Debug.Log("No match found.");
        }
    }

    // Finds matching adjacent tiles recursively.
    private void FindMatchingTiles(Tile tile, TileTypeAsset type, List<TileData> matches)
    {
        if (tile == null || matches.Contains(tile.Data)) return;
        if (tile.Type != type) return;

        matches.Add(tile.Data);
        int maxX = rows[0].tiles.Length;
        int maxY = rows.Count;

        if (tile.x > 0) FindMatchingTiles(rows[tile.y].tiles[tile.x - 1], type, matches);
        if (tile.x + 1 < maxX) FindMatchingTiles(rows[tile.y].tiles[tile.x + 1], type, matches);
        if (tile.y > 0) FindMatchingTiles(rows[tile.y - 1].tiles[tile.x], type, matches);
        if (tile.y + 1 < maxY) FindMatchingTiles(rows[tile.y + 1].tiles[tile.x], type, matches);
    }

    // Handles the match animation and replacement of matched tiles.
    private async Task HandleMatchAsync(MatchTiles match)
    {
        _isMatching = true;
        Debug.Log("Handling match animation");

        var tiles = match.Tiles.Select(t => rows[t.Y].tiles[t.X]).ToList();
        var deflateSequence = DOTween.Sequence();

        foreach (var tile in tiles)
        {
            deflateSequence.Join(tile.icon.transform.DOScale(Vector3.zero, tweenDuration).SetEase(Ease.InBack));
        }
        audioSource.PlayOneShot(matchSound);
        await deflateSequence.Play().AsyncWaitForCompletion();

        var inflateSequence = DOTween.Sequence();
        foreach (var tile in tiles)
        {
            tile.Type = tileTypes[UnityEngine.Random.Range(0, tileTypes.Length)];
            inflateSequence.Join(tile.icon.transform.DOScale(Vector3.one, tweenDuration).SetEase(Ease.OutBack));
        }

        await inflateSequence.Play().AsyncWaitForCompletion();
        Debug.Log("Match animation complete.");
        OnMatch?.Invoke(tileTypes.First(t => t.id == match.TypeId), match.Tiles.Length);
        _isMatching = false;
    }
}
