using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class TileDataMatrix : MonoBehaviour
{
    // Method to get the connections of a tile
    public static (TileData[], TileData[]) GetConnections(int originX, int originY, TileData[,] tiles)
    {
        var origin = tiles[originX, originY];

        var width = tiles.GetLength(0);
        var height = tiles.GetLength(1);

        var horizontalConnections = new List<TileData>();
        var verticalConnections = new List<TileData>();

        // Checking horizontally to the left
        for (var x = originX - 1; x >= 0; x--)
        {
            var other = tiles[x, originY];

            if (other.TypeId != origin.TypeId) break;

            horizontalConnections.Add(other);
        }

        // Checking horizontally to the right
        for (var x = originX + 1; x < width; x++)
        {
            var other = tiles[x, originY];

            if (other.TypeId != origin.TypeId) break;

            horizontalConnections.Add(other);
        }

        // Checking vertically upwards
        for (var y = originY - 1; y >= 0; y--)
        {
            var other = tiles[originX, y];

            if (other.TypeId != origin.TypeId) break;

            verticalConnections.Add(other);
        }

        // Checking vertically downwards
        for (var y = originY + 1; y < height; y++)
        {
            var other = tiles[originX, y];

            if (other.TypeId != origin.TypeId) break;

            verticalConnections.Add(other);
        }

        return (horizontalConnections.ToArray(), verticalConnections.ToArray());
    }

}
