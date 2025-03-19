//Base class for all tiles in the game. It has the common properties
using UnityEngine;

public class TileBase : MonoBehaviour
{
    public TileType tileType;
    public bool Clickable = true;
    public bool IsFallable = true;
    public bool InterectWithExplode = false;
    public int Health = 1;
    public FallAnimation FallAnimation;
}
