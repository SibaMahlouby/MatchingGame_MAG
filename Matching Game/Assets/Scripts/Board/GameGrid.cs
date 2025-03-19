using UnityEngine;

public class GameGrid : MonoBehaviour
{
    public Transform cellsParent;
    public Transform tilesParent;
    public Transform particlesParent;
    [SerializeField] private Cell cellPrefab;

    public LevelInfo levelInfo;

    public int Rows { get; private set; }
    public int Cols { get; private set; }

    public Cell[,] Cells { get; private set; }

    private void Awake()
    {
        LoadLevelInfo();
        InitializeCells();
        PrepareCells();
    }

    private void LoadLevelInfo()
    {
        int currentLevel = PlayerPrefs.GetInt("Level", 1);
        levelInfo = LevelManager.getLevelInfo(currentLevel);

        Rows = levelInfo.grid_height;
        Cols = levelInfo.grid_width;
    }

    private void InitializeCells()
    {
        Cells = new Cell[Cols, Rows];
        ResizeBoard(Rows, Cols);
        CreateCells();
    }

    private void CreateCells()
    {
        // Calculate the center offset
        float cellSize = 1.0f; // Assuming each cell is 1 unit in size
        float gridWidth = Cols * cellSize;
        float gridHeight = Rows * cellSize;
        Vector3 centerOffset = new Vector3(-gridWidth / 2 + cellSize / 2, gridHeight / 2 - cellSize / 2, 0);

        for (int y = 0; y < Rows; y++)
        {
            for (int x = 0; x < Cols; x++)
            {
                Vector3 cellPosition = new Vector3(x * cellSize, -y * cellSize, 0) + centerOffset;
                Cells[x, y] = Instantiate(cellPrefab, cellPosition, Quaternion.identity, cellsParent);
            }
        }
    }

    private void PrepareCells()
    {
        for (int y = 0; y < Rows; y++)
            for (int x = 0; x < Cols; x++)
                Cells[x, y].Prepare(x, y, this);
    }

    private void ResizeBoard(int rows, int cols)
    {
        Transform currTrans = this.transform;

        float newX = (9 - cols) * 0.5f;
        float newY = (9 - rows) * 0.5f;

        this.transform.position = new Vector3(newX, newY, currTrans.position.z);
    }
}