//Information and methods for single cell.
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

    public Tiles tile
    {
        get
        {
            return _tile;
        }
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

    private void UpdateLabel()
    {
        var cellName = X + " " + Y;
        labelText.text = cellName;
        gameObject.name = "Cell " + cellName;
    }

    private void UpdateNeighbours()
    {
        neighbours = GetNeighbours(Direction.Up, Direction.Down, Direction.Left, Direction.Right);
        firstCellBelow = GetNeighbourWithDirection(Direction.Down);
    }

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

    public void UpdateAllArea()
    {
        allArea = GetNeighbours(Direction.Up, Direction.UpRight, Direction.Right, Direction.DownRight, Direction.Down, Direction.DownLeft, Direction.Left, Direction.UpLeft);
    }

    public void CellTapped()
    {
        if (tile == null) return;

        SpecialTapSwitcher();
    }

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

    public Cell GetFallTarget()
    {
        var targetCell = this;
        if (targetCell.firstCellBelow != null && targetCell.firstCellBelow.tile == null)
            targetCell = targetCell.firstCellBelow;

        return targetCell;
    }
}
