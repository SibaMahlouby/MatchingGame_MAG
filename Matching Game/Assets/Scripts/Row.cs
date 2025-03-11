using UnityEngine;

    public sealed class Row : MonoBehaviour
    {
        [HideInInspector] public Tile[] tiles;

        public void InitializeRow(int width, GameObject tilePrefab, Transform parent)
        {
            // 🛑 FIX: Ensure no pre-existing tiles before creating new ones
            foreach (Transform child in parent)
            {
                Destroy(child.gameObject); // Remove extra duplicate tiles
            }

            tiles = new Tile[width];

            for (int x = 0; x < width; x++)
            {
                GameObject tileObj = Instantiate(tilePrefab, parent); // 👈 Spawning tiles properly
                Tile tile = tileObj.GetComponent<Tile>();
                tile.x = x;
                tile.y = transform.GetSiblingIndex(); // Set row index
                tiles[x] = tile;
            }
        }
    }