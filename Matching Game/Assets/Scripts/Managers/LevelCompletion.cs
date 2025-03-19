//it listens to the OnGoalsCompleted event from GoalManager.
using UnityEngine;

public class LevelCompletion : MonoBehaviour
{
    [SerializeField] private GameGrid board;

    private void OnEnable()
    {
        GoalManager.Instance.OnGoalsCompleted += HandleGoalsCompleted;
    }

    private void OnDisable()
    {
        GoalManager.Instance.OnGoalsCompleted -= HandleGoalsCompleted;
    }

    private void HandleGoalsCompleted()
    {
        UIManager.Instance.SetLevelCompletedPanel();
    }
}
