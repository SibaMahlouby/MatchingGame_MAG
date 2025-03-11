using UnityEngine;

    public sealed class Row : MonoBehaviour
    {
        [HideInInspector] public Tile[] tiles;

    // Initializes the row with the specified number of tiles and assigns them to this row.
    public void InitializeRow(int width, GameObject tilePrefab, Transform parent)
        {
           // Clean up 
           foreach (Transform child in parent)
            {
                Destroy(child.gameObject); 
            }

            tiles = new Tile[width];
        // Create and place each tile in the row
        for (int x = 0; x < width; x++)
            {
                GameObject tileObj = Instantiate(tilePrefab, parent); 
                Tile tile = tileObj.GetComponent<Tile>();
                tile.x = x;
                tile.y = transform.GetSiblingIndex(); 
                tiles[x] = tile;
            }
        }
    }