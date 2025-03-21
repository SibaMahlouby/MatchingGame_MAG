// Information and methods for a single cell in the grid.

using System;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public TextMesh labelText;

    [HideInInspector] public int X;
    [HideInInspector] public int Y;

    public List<Cell> neighbours { get; private set; }
    public List<Cell> allArea { get; private set; }

    [HideInInspector] public Cell firstCellBelow;
    [HideInInspector] public bool isFillingCell;

    private Tiles _tile;

    public GameGrid gameGrid { get; private set; }

    // Manages the tile in this cell and ensures proper references.
    public Tiles tile
    {
        get { return _tile; }
        set
        {
            if (_tile == value) return;

            var oldTile = _tile;
            _tile = value;

            if (oldTile != null && Equals(oldTile.Cell, this))
                oldTile.Cell = null;

            if (value != null)
                value.Cell = this;
        }
    }

    // Initializes the cell with its position and grid reference.
    public void Prepare(int x, int y, GameGrid board)
    {
        gameGrid = board;
        X = x;
        Y = y;
        transform.localPosition = new Vector3(x, y);
        isFillingCell = (Y == gameGrid.Rows - 1);

        UpdateLabel();
        UpdateNeighbours();
        UpdateAllArea();
    }

    // Updates the cell's label with its coordinates.
    private void UpdateLabel()
    {
        var cellName = X + " " + Y;
        labelText.text = cellName;
        gameObject.name = "Cell " + cellName;
    }

    // Updates the list of direct neighbors and the cell below.
    private void UpdateNeighbours()
    {
        neighbours = GetNeighbours(Direction.Up, Direction.Down, Direction.Left, Direction.Right);
        firstCellBelow = GetNeighbourWithDirection(Direction.Down);
    }

    // Returns a list of neighbors in the specified directions.
    private List<Cell> GetNeighbours(params Direction[] directions)
    {
        var neighbours = new List<Cell>();

        foreach (var direction in directions)
        {
            var neighbour = GetNeighbourWithDirection(direction);
            if (neighbour != null)
            {
                neighbours.Add(neighbour);
            }
        }

        return neighbours;
    }

    // Returns the neighbor in the specified direction.
    private Cell GetNeighbourWithDirection(Direction direction)
    {
        var x = X;
        var y = Y;
        switch (direction)
        {
            case Direction.None: break;
            case Direction.Right: x += 1; break;
            case Direction.Left: x -= 1; break;
            case Direction.UpRight: x += 1; y += 1; break;
            case Direction.DownRight: x += 1; y -= 1; break;
            case Direction.UpLeft: x -= 1; y += 1; break;
            case Direction.DownLeft: x -= 1; y -= 1; break;
            case Direction.Up: y += 1; break;
            case Direction.Down: y -= 1; break;
            default: throw new ArgumentOutOfRangeException("direction", direction, null);
        }

        if (x >= gameGrid.Cols || x < 0 || y >= gameGrid.Rows || y < 0) return null;

        return gameGrid.Cells[x, y];
    }

    // Updates the list of all surrounding cells (including diagonals).
    public void UpdateAllArea()
    {
        allArea = GetNeighbours(Direction.Up, Direction.UpRight, Direction.Right, Direction.DownRight, Direction.Down, Direction.DownLeft, Direction.Left, Direction.UpLeft);
    }

    // Handles player taps on the cell.
    public void CellTapped()
    {
        if (tile == null) return;

        SpecialTapSwitcher();
    }

    // Switches behavior based on the tile's match type.
    private void SpecialTapSwitcher()
    {
        switch (tile.GetMatchType())
        {
            case MatchType.Special:
                ComboManager.Instance.TryExecute(this);
                break;
            default:
                MatchingManager.Instance.ExplodeMatchingCells(this);
                break;
        }
    }

    // Returns the target cell for falling tiles.
    public Cell GetFallTarget()
    {
        var targetCell = this;
        if (targetCell.firstCellBelow != null && targetCell.firstCellBelow.tile == null)
            targetCell = targetCell.firstCellBelow;

        return targetCell;
    }
}