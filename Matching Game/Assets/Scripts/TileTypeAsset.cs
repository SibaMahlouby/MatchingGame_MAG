
using UnityEngine;

// Create an asset menu item in Unity Editor for easy creation of TileTypeAsset objects.

[CreateAssetMenu(menuName = "Matching Game MAG/ Tile")]
public sealed class TileTypeAsset : ScriptableObject
{
    public int id;

    public int value;

    public Sprite sprite;
}