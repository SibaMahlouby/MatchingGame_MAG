//For Managing the fall animation of a tile.

using UnityEngine;
using DG.Tweening;


public class FallAnimation : MonoBehaviour
{
    public Tiles tile; // The tile that will fall
    [HideInInspector] public Cell targetCell; // The target cell where the tile should land

    [SerializeField] private const float ANIMATION_DURATION = 0.35f; 
    private Vector3 targetPosition;

    // Set the maximum number of tweens for optimization.
    private void Awake()
    {
        DOTween.SetTweensCapacity(500, 50);
    }

    // Moves the tile to the specified target cell with a fall animation.
    public void FallTo(Cell targetCell)
    {
        if (IsInvalidTargetCell(targetCell)) return;

        UpdateTargetCell(targetCell);
        AnimateFall();
    }

    
    //Validates if the target cell is a valid destination.
    private bool IsInvalidTargetCell(Cell targetCell)
    {
        return this.targetCell != null && targetCell.Y >= this.targetCell.Y;
    }

   
    // Updates the target cell and sets the new tile position.
   
    private void UpdateTargetCell(Cell targetCell)
    {
        this.targetCell = targetCell;
        tile.Cell = this.targetCell;
        targetPosition = this.targetCell.transform.position;
    }

    //Animates the tile's fall to the target position using a smooth cubic easing function.
    private void AnimateFall()
    {
        tile.transform.DOMoveY(targetPosition.y, ANIMATION_DURATION)
            .SetEase(Ease.InCubic)
            .OnComplete(() => targetCell = null); // Clears the reference after completing the fall
    }
}
