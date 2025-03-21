//Manages user touch inputs for both mobile and editor mouse interactions.
//Handles cell taps and performs actions when a cell is tapped.

using UnityEngine;

public class TouchManager : MonoBehaviour
{
    private const string cellCollider = "CellCollider";

    [SerializeField] private new Camera camera;
    [SerializeField] private GameGrid board;

    private void Update()
    {
        #if UNITY_EDITOR
            GetTouchEditor();
        #else
            GetTouchMobile();
        #endif
    }

    //Handles touch input when running in the Unity Editor
    private void GetTouchEditor()
    {
        if (Input.GetMouseButtonUp(0))
        {
            ExecuteTouch(Input.mousePosition);
        }
    }


    //Handles touch input on mobile devices.
    private void GetTouchMobile()
    {
        var touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                ExecuteTouch(touch.position);
                break;
        }
    }

    //Executes touch logic by converting screen position to world position and checking for tapped cells.
    private void ExecuteTouch(Vector3 pos)
    {
        var hit = Physics2D.OverlapPoint(camera.ScreenToWorldPoint(pos)) as BoxCollider2D;
        if (hit != null && hit.CompareTag(cellCollider))
        {
            hit.GetComponent<Cell>().CellTapped();
        }
    }

    
    private void DisableTouch()
    {
        this.enabled = false;
    }

    private void OnEnable()
    {
        MovesManager.Instance.OnMovesFinished += DisableTouch;
        GoalManager.Instance.OnGoalsCompleted += DisableTouch;
    }

    private void OnDisable()
    {
        MovesManager.Instance.OnMovesFinished -= DisableTouch;
        GoalManager.Instance.OnGoalsCompleted -= DisableTouch;
    }
}
