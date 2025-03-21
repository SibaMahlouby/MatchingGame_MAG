//Responsible for managing the "falling" and "filling" mechanics of the game.

using System.Collections.Generic;

public class FallAndFillManager : Singleton<FallAndFillManager>
{
    private bool isActive;
    private GameGrid board;
    private LevelData levelData;
    private Cell[] fillingCells;

    // Initialization of the FallAndFillManager with board and level data.

    public void Init(GameGrid board, LevelData levelData)
    {
        this.board = board;
        this.levelData = levelData;

        FindFillingCells();
        StartFall();
    }


    //identify all the cells in the grid that need filling.
    public void FindFillingCells()
    {
        var cellList = new List<Cell>();

        for (var y = 0; y < board.Rows; y++)
        {
            for (var x = 0; x < board.Cols; x++)
            {
                var cell = board.Cells[x, y];

                if (cell != null && cell.isFillingCell)
                    cellList.Add(cell);
            }
        }
        fillingCells = cellList.ToArray();
    }

    // Perform the falling mechanic for all the tiles in the grid.
    public void DoFalls()
    {
        for (int y = 0; y < board.Rows; y++)
        {
            for (int x = 0; x < board.Cols; x++)
            {
                var cell = board.Cells[x, y];

                if (cell.tile != null && cell.firstCellBelow != null && cell.firstCellBelow.tile == null)
                    cell.tile.Fall();
            }
        }
    }

    //Fill the empty cells with new tiles.
    public void DoFills()
    {
        for (int i = 0; i < fillingCells.Length; i++)
        {
            var cell = fillingCells[i];

            if (cell.tile == null)
            {
                cell.tile = TileFactory.Instance.CreateTile(LevelData.GetRandomCubeTileType(), board.tilesParent);

                var offsetY = 0.0f;
                var targetCellBelow = cell.GetFallTarget().firstCellBelow;

                if (targetCellBelow != null)
                {
                    if (targetCellBelow.tile != null)
                    {
                        offsetY = targetCellBelow.tile.transform.position.y + 1;
                    }
                }

                var pos = cell.transform.position;
                pos.y += 2;
                pos.y = pos.y > offsetY ? pos.y : offsetY;

                if (cell.tile == null) continue;

                cell.tile.transform.position = pos;
                cell.tile.Fall();
            }
        }
    }
    public void StartFall() { isActive = true; }
    public void StopFall() { isActive = false; }

    private void Update()
    {
        if (!isActive) return;

        DoFalls();
        DoFills();

    }
}
