//Storing all the tiles images in the game.
using UnityEngine;


public class TilesImageLibrary : Singleton<TilesImageLibrary>
{
    [Header("Cubes")]
    public Sprite GreenCubeSprite;
    public Sprite YellowCubeSprite;
    public Sprite BlueCubeSprite;
    public Sprite RedCubeSprite;
    [Header("Obstacles")]
    public Sprite BoxSprite;


    [Header("TNT")]
    public Sprite TNTSprite;


    public Sprite GetSpriteForTileType(TileType tileType)
    {
        switch (tileType)
        {
            // Cubes
            case TileType.GreenCube: return GreenCubeSprite;
            case TileType.YellowCube: return YellowCubeSprite;
            case TileType.BlueCube: return BlueCubeSprite;
            case TileType.RedCube: return RedCubeSprite;
            // Obstacles
            case TileType.Box: return BoxSprite;

            // TNT
            case TileType.TNT: return TNTSprite;

            default: return null;
        }
    }
}
