// Identifying, counting, and exploding matching tiles

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchingManager : Singleton<MatchingManager>
{
    [SerializeField] private GameGrid board; 

    private bool[,] visitedCells; 
    private int minimumMatchCount = 2; 

    public void Start()
    {
        visitedCells = new bool[board.Cols, board.Rows]; 
    }

    // Finds matches of a given match type starting from a specific cell.
    public List<Cell> FindMatches(Cell cell, MatchType matchType)
    {
        var matchedCells = new List<Cell>();
        ClearVisitedCells(); 
        FindMatches(cell, matchType, matchedCells); 

        return matchedCells;
    }

    // Recursively finds all matching cells from the given starting cell.
    public void FindMatches(Cell cell, MatchType matchType, List<Cell> matchedCells)
    {
        if (cell == null) return;

        var x = cell.X;
        var y = cell.Y;

        if (visitedCells[x, y]) return; 

        // Check if the cell's tile matches the given match type.
        if (cell.tile != null && cell.tile.GetMatchType() == matchType && cell.tile.GetMatchType() != MatchType.None)
        {
            visitedCells[x, y] = true; // Mark the cell as visited.
            matchedCells.Add(cell); // Add the cell to the matched cells list.

            if (!cell.tile.Clickable) return; // Skip non-clickable tiles.

            var neighbours = cell.neighbours; // Get the cell's neighbors.

            if (neighbours.Count == 0) return;

            // find matches in neighboring cells.
            for (int i = 0; i < neighbours.Count; i++)
            {
                FindMatches(neighbours[i], matchType, matchedCells);
            }
        }
    }

    // Clears the visited cells array.
    private void ClearVisitedCells()
    {
        for (int x = 0; x < visitedCells.GetLength(0); x++)
        {
            for (int y = 0; y < visitedCells.GetLength(1); y++)
            {
                visitedCells[x, y] = false;
            }
        }
    }

    // Counts the number of matched cube tiles in a list of cells.
    public int CountMatchedCubeTile(List<Cell> cells)
    {
        int count = 0;
        foreach (Cell cell in cells)
        {
            if (cell.tile.Clickable) 
                count++;
        }
        return count;
    }

    // Explodes matching tiles starting from a specific cell.
    public void ExplodeMatchingCells(Cell cell)
    {
        var previousCells = new List<Cell>(); // Tracks cells already processed.

        // Find all matching cells.
        var matchedCells = FindMatches(cell, cell.tile.GetMatchType());
        var matchedCubeTileCount = CountMatchedCubeTile(matchedCells);

        if (matchedCubeTileCount < minimumMatchCount) return; 

        // Explode all matched tiles.
        for (int i = 0; i < matchedCells.Count; i++)
        {
            var explodedCell = matchedCells[i];

            ExplodeMatchingCellsInNeightbours(explodedCell, previousCells);

            var tile = explodedCell.tile;
            tile.TryExecute(); 
        }

        _ = MovesManager.Instance.DecreaseMovesAsync(); // Decrease the move count.
        SpawnBonus(cell, matchedCubeTileCount); 
    }

    // Spawns a bonus tile (TNT) based on the size of the match.
    private void SpawnBonus(Cell cell, int matchedCellCount)
    {
        switch (matchedCellCount)
        {
            case int n when n >= 5: // Spawn TNT for large matches.
                cell.tile = TileFactory.Instance.CreateTile(TileType.TNT, board.tilesParent);
                break;

            default:
                break;
        }

        if (cell.tile == null) return;
        cell.tile.transform.position = cell.transform.position; // Position the bonus tile.
    }

    // Explodes matching tiles in neighboring cells.
    private void ExplodeMatchingCellsInNeightbours(Cell cell, List<Cell> previousCells)
    {
        var explodedCellNeightbours = cell.neighbours; // Get the cell's neighbors.

        for (int j = 0; j < explodedCellNeightbours.Count; j++)
        {
            var neighbourCell = explodedCellNeightbours[j];
            var neighbourCellTile = neighbourCell.tile;

            // Skip already processed cells or cells without tiles.
            if (neighbourCellTile != null && !previousCells.Contains(neighbourCell))
            {
                previousCells.Add(neighbourCell); // Mark the cell as processed.

                // Trigger the tile's execution logic if it interacts with explosions.
                if (neighbourCellTile.InterectWithExplode)
                    neighbourCellTile.TryExecute();
            }
        }
    }
}