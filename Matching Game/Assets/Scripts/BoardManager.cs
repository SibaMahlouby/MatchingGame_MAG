using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class BoardManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Transform boardParent;
    [SerializeField] private TileTypeAsset[] tileTypes;
    [SerializeField] private AudioClip matchSound;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float tweenDuration;
    [SerializeField] private bool ensureNoStartingMatches;
    [SerializeField] private int width;
    [SerializeField] private int height;


    private bool _isMatching;
    private List<Row> rows = new List<Row>();

    public event Action<TileTypeAsset, int> OnMatch;
     void Start()
    {

        GenerateBoard(width, height);
    }

    public void GenerateBoard(int width, int height)
    {
        ClearBoard(); // Remove previous tiles if any

        for (int y = 0; y < height; y++)
        {
            // Instantiate rowPrefab
            GameObject rowObj = Instantiate(rowPrefab, boardParent);
            Row row = rowObj.GetComponent<Row>();
            row.InitializeRow(width, tilePrefab, rowObj.transform);
            rows.Add(row);
        }

        AssignTileTypes();
    }



    // 🎯 NEW: Clears existing board before creating a new one
    private void ClearBoard()
    {
        foreach (var row in rows)
        {
            Destroy(row.gameObject);
        }
        rows.Clear();
    }
    private void AssignTileTypes()
    {
        for (int y = 0; y < rows.Count; y++)
        {
            var row = rows[y];
            for (int x = 0; x < row.tiles.Length; x++)
            {
                var tile = row.tiles[x];

                // Manually assign x and y coordinates
                tile.x = x;  // x is the index in the row
                tile.y = y;  // y is the index in the rows

                tile.Type = tileTypes[UnityEngine.Random.Range(0, tileTypes.Length)];
                tile.button.onClick.AddListener(() => Select(tile));
            }
        }
    }





    private async void Select(Tile tile)
    {
        if (_isMatching || tile == null) return;

        Debug.Log($"Tile clicked at x: {tile.x}, y: {tile.y}");

        // ✅ FIX: Ensure the tile is within bounds before checking for matches
        if (tile.x < 0 || tile.y < 0 || tile.y >= rows.Count || tile.x >= rows[tile.y].tiles.Length)
        {
            Debug.LogWarning("Tile clicked is out of valid board range!");
            return;
        }

        List<TileData> matchingTiles = new List<TileData>();
        FindMatchingTiles(tile, tile.Type, matchingTiles);

        if (matchingTiles.Count > 1) // If a match is found
        {
            await HandleMatchAsync(new MatchTiles(tile.Data, matchingTiles.ToArray(), new TileData[0]));
        }
    }





    private void FindMatchingTiles(Tile tile, TileTypeAsset type, List<TileData> matches)
    {
        if (tile == null || matches.Contains(tile.Data)) return;

        int maxX = rows[0].tiles.Length; // Total columns
        int maxY = rows.Count;           // Total rows

        if (tile.Type == type)
        {
            matches.Add(tile.Data);

            // ✅ FIX: Only access neighbors if they are within bounds
            if (tile.x > 0) // Left
            {
                if (tile.x - 1 >= 0 && tile.x - 1 < maxX)
                    FindMatchingTiles(rows[tile.y].tiles[tile.x - 1], type, matches);
            }

            if (tile.x + 1 < maxX) // Right
            {
                FindMatchingTiles(rows[tile.y].tiles[tile.x + 1], type, matches);
            }

            if (tile.y > 0) // Down
            {
                if (tile.y - 1 >= 0 && tile.y - 1 < maxY)
                    FindMatchingTiles(rows[tile.y - 1].tiles[tile.x], type, matches);
            }

            if (tile.y + 1 < maxY) // Up
            {
                if (tile.y + 1 >= 0 && tile.y + 1 < maxY)
                    FindMatchingTiles(rows[tile.y + 1].tiles[tile.x], type, matches);
            }
        }
    }



    // 🎯 NEW: Handles matches with animations
    private async Task HandleMatchAsync(MatchTiles match)
    {
        _isMatching = true;

        var tiles = match.Tiles.Select(t => rows[t.Y].tiles[t.X]).ToList();

        // Animate matched tiles shrinking
        var deflateSequence = DOTween.Sequence();
        foreach (var tile in tiles)
        {
            deflateSequence.Join(tile.icon.transform.DOScale(Vector3.zero, tweenDuration).SetEase(Ease.InBack));
        }
        audioSource.PlayOneShot(matchSound);
        await deflateSequence.Play().AsyncWaitForCompletion();

        // Replace matched tiles with new ones
        var inflateSequence = DOTween.Sequence();
        foreach (var tile in tiles)
        {
            tile.Type = tileTypes[UnityEngine.Random.Range(0, tileTypes.Length)];
            inflateSequence.Join(tile.icon.transform.DOScale(Vector3.one, tweenDuration).SetEase(Ease.OutBack));
        }

        await inflateSequence.Play().AsyncWaitForCompletion();

        OnMatch?.Invoke(tileTypes.First(t => t.id == match.TypeId), match.Tiles.Length);
        _isMatching = false;
    }
}

