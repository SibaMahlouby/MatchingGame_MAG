//Is responsible for managing the game level and creating the tiles in the grid

using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameGrid gameGrid;
    [SerializeField] private FallAndFillManager fallAndFillManager;
    [SerializeField] private GoalManager goalManager;
    [SerializeField] private MovesManager movesManager;
    private LevelData levelData;

    private void Start()
    {
        PrepareLevel();
        InitFallAndFills();
        movesManager.Init(levelData.Moves);
        goalManager.Init(levelData.Goals);

    }

    private void PrepareLevel()
    {
        levelData = new LevelData(gameGrid.levelInfo);

        for (int i = 0; i < gameGrid.levelInfo.grid_height; ++i)
            for (int j = 0; j < gameGrid.levelInfo.grid_width; ++j)
            {
                var cell = gameGrid.Cells[j, i];

                var tileType = levelData.GridData[gameGrid.levelInfo.grid_height - i - 1, j];
                var tile = TileFactory.Instance.CreateTile(tileType, gameGrid.tilesParent);
                if (tile == null) continue;

                cell.tile = tile;
                tile.transform.position = cell.transform.position;

            }
    }

    private void InitFallAndFills()
    {
        FallAndFillManager.Instance.Init(gameGrid, levelData);
    }

    public static LevelInfo getLevelInfo(int level)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Levels/level_" + level.ToString("00"));
        string jsonString = jsonFile.text;
        return JsonUtility.FromJson<LevelInfo>(jsonString);
    }
}
