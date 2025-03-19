using System.Collections.Generic;

public class FallAndFillManager : Singleton<FallAndFillManager>
{
    private bool isActive;
    private GameGrid board;
    private LevelData levelData;
    private Cell[] fillingCells;
    public void Init(GameGrid board, LevelData levelData)
    {
        this.board = board;
        this.levelData = levelData;

        FindFillingCells();
        StartFall();
    }

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
