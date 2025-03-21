//responsible for creating different types of game tiles based on the TileType.

using System;
using System.Collections.Generic;
using UnityEngine;

public class TileFactory : Singleton<TileFactory>
{
    public TileBase TileBasePrefab;

    // Dictionary to map TileType to its respective creation function
    private Dictionary<TileType, Func<TileBase, Tiles>> tileCreators = new Dictionary<TileType, Func<TileBase, Tiles>>
    {
        { TileType.GreenCube, (tileBase) => CreateCubeTile(tileBase, MatchType.Green) },
        { TileType.BlueCube, (tileBase) => CreateCubeTile(tileBase, MatchType.Blue) },
        { TileType.RedCube, (tileBase) => CreateCubeTile(tileBase, MatchType.Red) },
        { TileType.YellowCube, (tileBase) => CreateCubeTile(tileBase, MatchType.Yellow) },
        { TileType.Box, CreateBoxTile },
        { TileType.TNT, CreateTNTTile }
    };

    // Creates a tile based on the specified TileType and parent transform
    public Tiles CreateTile(TileType tileType, Transform parent)
    {
        if (tileType == TileType.None) return null;

        var tileBase = Instantiate(TileBasePrefab, Vector3.zero, Quaternion.identity, parent);
        tileBase.tileType = tileType;

        if (!tileCreators.TryGetValue(tileType, out var createTile))
        {
            Debug.LogWarning("Can not create tile: " + tileType);
            return null;
        }

        return createTile(tileBase);
    }

    //Create a CubeTile with a specific match type
    private static Tiles CreateCubeTile(TileBase tileBase, MatchType matchType)
    {
        var cubeTile = tileBase.gameObject.AddComponent<CubeTile>();
        cubeTile.PrepareCubeTile(tileBase, matchType);
        return cubeTile;
    }

    //Create a TNTTile
    private static Tiles CreateTNTTile(TileBase tileBase)
    {
        var tntTile = tileBase.gameObject.AddComponent<TNTTile>();
        tntTile.PrepareTNTTile(tileBase);
        return tntTile;
    }


    private static Tiles CreateBoxTile(TileBase tileBase)
    {
        var boxTile = tileBase.gameObject.AddComponent<BoxTile>();
        boxTile.PrepareBoxTile(tileBase);
        return boxTile;
    }
}
