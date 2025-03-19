//It represents the data of a level in the game, where it takes a LevelInfo object and creates the level data

using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelData
{

    public TileType[,] GridData { get; protected set; }
    public List<LevelGoal> Goals { get; protected set; }
    public int Moves { get; protected set; }

    public LevelData(LevelInfo levelInfo)
    {
        // Count obstacles for goal data
        int numberOfBoxes = 0;

        // Set the grid data
        GridData = new TileType[levelInfo.grid_height, levelInfo.grid_width];

        int gridIndex = 0;
        for (int i = levelInfo.grid_height - 1; i >= 0; --i)
            for (int j = 0; j < levelInfo.grid_width; ++j)
            {
                switch (levelInfo.grid[gridIndex++])
                {
                    // Obstacles
                    case "bo":
                        GridData[i, j] = TileType.Box;
                        ++numberOfBoxes;
                        break;
                    // Cubes
                    case "b":
                        GridData[i, j] = TileType.BlueCube;
                        break;
                    case "g":
                        GridData[i, j] = TileType.GreenCube;
                        break;
                    case "r":
                        GridData[i, j] = TileType.RedCube;
                        break;
                    case "y":
                        GridData[i, j] = TileType.YellowCube;
                        break;
                    case "rand":
                        GridData[i, j] = ((TileType[])Enum.GetValues(typeof(TileType)))[Random.Range(1, 5)];
                        break;
                    case "t":
                        GridData[i, j] = TileType.TNT;
                        break;
                    default:
                        GridData[i, j] = ((TileType[])Enum.GetValues(typeof(TileType)))[Random.Range(1, 5)];
                        break;
                }
            }

        // Set the goals data
        Goals = new List<LevelGoal>();
        if (numberOfBoxes != 0) Goals.Add(new LevelGoal { TileType = TileType.Box, Count = numberOfBoxes });

        // Set moves
        Moves = levelInfo.move_count;
    }

    public static TileType GetRandomCubeTileType()
    {
        return ((TileType[])Enum.GetValues(typeof(TileType)))[Random.Range(1, 5)]; // 1,5 represents number of blocks
    }

}
