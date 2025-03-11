using UnityEngine;
using UnityEngine.UI;

   public sealed class Tile : MonoBehaviour
    {
        public int x;
        public int y;
        public Image icon; // Assign this in Unity
        public Button button;

        private TileTypeAsset _type;

        public TileTypeAsset Type
        {
            get => _type;
            set
            {
                if (_type == value) return;
                _type = value;
                icon.sprite = _type.sprite; // Assign correct image
            }
        }

        public TileData Data => new TileData(x, y, _type.id);
    }