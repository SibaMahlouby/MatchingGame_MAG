//Manages the initialization and setup of the game level. This includes grid creation, tile placement, goal management, and move tracking.

using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameGrid gameGrid;
    [SerializeField] private FallAndFillManager fallAndFillManager;
    [SerializeField] private GoalManager goalManager;
    [SerializeField] private MovesManager movesManager;
    private LevelData levelData;

    // Initializes the level by setting up tiles, goals, and move management.
    private void Start()
    {
        PrepareLevel();
        InitFallAndFills();
        movesManager.Init(levelData.Moves);
        goalManager.Init(levelData.Goals);

    }

    // Loads level data and initializes the game grid with tiles.
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

    //Initializes the fall and fill manager, which handles tile movement.
    private void InitFallAndFills()
    {
        FallAndFillManager.Instance.Init(gameGrid, levelData);
    }

    //Loads the level information from a JSON file based on the level number.
    public static LevelInfo getLevelInfo(int level)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Levels/level_" + level.ToString("00"));
        string jsonString = jsonFile.text;
        return JsonUtility.FromJson<LevelInfo>(jsonString);
    }
}
