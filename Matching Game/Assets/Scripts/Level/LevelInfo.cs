//To store the information of a level. It is compatible with JSON format

[System.Serializable]
public class LevelInfo
{
    public int level_number;
    public int grid_width;
    public int grid_height;
    public int move_count;
    public string[] grid;
}
