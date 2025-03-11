public sealed class MatchTiles
{
    public readonly int TypeId; 

    public readonly int Score; 

    public readonly TileData[] Tiles; // Array of tiles that are part of the match

    
    public MatchTiles(TileData origin, TileData[] horizontal, TileData[] vertical)
    {
        TypeId = origin.TypeId; 

        // Check if the horizontal and vertical matches have enough tiles and hold them all
        if (horizontal.Length >= 2 && vertical.Length >= 2)
        {
            Tiles = new TileData[horizontal.Length + vertical.Length + 1];
            Tiles[0] = origin; 

            horizontal.CopyTo(Tiles, 1);
            vertical.CopyTo(Tiles, horizontal.Length + 1);
        }
        else if (horizontal.Length >= 2)
        {
            Tiles = new TileData[horizontal.Length + 1];
            Tiles[0] = origin;
            horizontal.CopyTo(Tiles, 1);
        }
        else if (vertical.Length >= 2)
        {
            Tiles = new TileData[vertical.Length + 1];
            Tiles[0] = origin;
            vertical.CopyTo(Tiles, 1);
        }
        else
        {
            Tiles = null;
        }

        Score = Tiles?.Length ?? -1;
    }
}
