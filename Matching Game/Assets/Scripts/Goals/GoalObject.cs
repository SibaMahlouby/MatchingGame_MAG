//It represents a goal in a level.

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoalObject : MonoBehaviour
{
    [SerializeField] private Image goalImage; 
    [SerializeField] private Image completedMarkImage; 
    [SerializeField] private TextMeshProUGUI goalCountText; 

    private int goalCount; 
    private LevelGoal levelGoal; // The level goal data.

    public LevelGoal LevelGoal => levelGoal; // Public getter for the level goal data.

    // Initializes the goal with its data and visual representation.
    public void Prepare(LevelGoal goal)
    {
        levelGoal = goal;
        var goalSprite = TilesImageLibrary.Instance.GetSpriteForTileType(levelGoal.TileType); 
        goalImage.sprite = goalSprite; // Set the goal's icon.

        goalCount = levelGoal.Count; // Initialize the remaining count.
        UpdateGoalCountText(); 
    }

    // Decreases the goal's remaining count and updates its state.
    public void DecreaseCount()
    {
        goalCount--;
        UpdateGoalState(); 
    }

    // Updates the goal's state based on the remaining count.
    private void UpdateGoalState()
    {
        if (goalCount <= 0)
        {
            MarkGoalAsCompleted(); 
            return;
        }

        UpdateGoalCountText(); 
    }

    // Updates the goal's count text in the UI.
    private void UpdateGoalCountText()
    {
        goalCountText.text = goalCount.ToString();
    }

    // Marks the goal as completed and updates the UI.
    private void MarkGoalAsCompleted()
    {
        goalCount = 0;
        goalCountText.gameObject.SetActive(false); 
        completedMarkImage.gameObject.SetActive(true); 
    }

    // Checks if the goal is completed.
    public bool IsCompleted()
    {
        return goalCount <= 0;
    }
}