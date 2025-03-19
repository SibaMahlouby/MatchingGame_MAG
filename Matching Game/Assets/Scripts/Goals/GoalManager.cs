//To manage the goals in a level.

using System;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : Singleton<GoalManager>
{
    [SerializeField] private GoalObject goalPrefab;
    [SerializeField] private Transform goalsParent;
    private List<GoalObject> goalObjects = new List<GoalObject>();

    public Action OnGoalsCompleted;
    private bool allGoalsCompleted = false;
    public void Init(List<LevelGoal> goals)
    {
        foreach (LevelGoal goal in goals)
        {
            GoalObject goalObject = Instantiate(goalPrefab, goalsParent);
            goalObject.Prepare(goal);
            goalObjects.Add(goalObject);
        }
    }

    public void UpdateLevelGoal(TileType tileType)
    {
        if (allGoalsCompleted) return;

        var goalObject = goalObjects.Find(goal => goal.LevelGoal.TileType.Equals(tileType));

        if (goalObject != null)
        {
            goalObject.DecreaseCount();
            CheckAllGoalsCompleted();
        }

    }

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