//Listens to the OnGoalsCompleted event from GoalManager and triggers level completion UI.
using UnityEngine;

public class LevelCompletion : MonoBehaviour
{
    [SerializeField] private GameGrid board;

    private void OnEnable()
    {
        if (GoalManager.Instance != null)
        {

            GoalManager.Instance.OnGoalsCompleted += HandleGoalsCompleted;
        }
    }

    private void OnDisable()
    {
        if (GoalManager.Instance != null)
        {

            GoalManager.Instance.OnGoalsCompleted -= HandleGoalsCompleted;
        }
    }

    private void HandleGoalsCompleted()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.SetLevelCompletedPanel();
        }
    }
}