//Manages the goals within a level.

using System;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : Singleton<GoalManager>
{
    [SerializeField] private GoalObject goalPrefab; // Prefab for creating goal UI objects.
    [SerializeField] private Transform goalsParent; 

    private List<GoalObject> goalObjects = new List<GoalObject>(); // List of active goal objects.
    public Action OnGoalsCompleted; 

    private bool allGoalsCompleted = false; 

    // Initializes the level goals by creating corresponding goal objects.
    public void Init(List<LevelGoal> goals)
    {
        foreach (LevelGoal goal in goals)
        {
            GoalObject goalObject = Instantiate(goalPrefab, goalsParent); 
            goalObject.Prepare(goal); // Set up the goal object with the level goal data.
            goalObjects.Add(goalObject); // Add the goal object to the list.
        }
    }

    // Updates the goal progress based on a matched tile type.
    public void UpdateLevelGoal(TileType tileType)
    {
        if (allGoalsCompleted) return; // Prevent further updates if goals are already completed.

        // Find the goal object that matches the tile type.
        var goalObject = goalObjects.Find(goal => goal.LevelGoal.TileType.Equals(tileType));

        if (goalObject != null)
        {
            goalObject.DecreaseCount(); 
            CheckAllGoalsCompleted(); 
        }
    }

    // Checks whether all goals have been completed.
    public bool CheckAllGoalsCompleted()
    {
        foreach (GoalObject goal in goalObjects)
        {
            if (!goal.IsCompleted()) 
                return false;
        }

        allGoalsCompleted = true; 
        OnGoalsCompleted?.Invoke(); 
        return true;
    }
}