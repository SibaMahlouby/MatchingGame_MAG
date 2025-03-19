//For Managing the fall animation of a tile.

using UnityEngine;
using DG.Tweening;

public class FallAnimation : MonoBehaviour
{
    public Tiles tile;
    [HideInInspector] public Cell targetCell;

    [SerializeField] private const float ANIMATION_DURATION = 0.35f;
    private Vector3 targetPosition;

    public void Awake()
    {
        DOTween.SetTweensCapacity(500, 50);
    }

    public void FallTo(Cell targetCell)
    {
        if (IsInvalidTargetCell(targetCell)) return;

        UpdateTargetCell(targetCell);
        AnimateFall();
    }

    private bool IsInvalidTargetCell(Cell targetCell)
    {
        return this.targetCell != null && targetCell.Y >= this.targetCell.Y;
    }

    private void UpdateTargetCell(Cell targetCell)
    {
        this.targetCell = targetCell;
        tile.Cell = this.targetCell;
        targetPosition = this.targetCell.transform.position;
    }

    private void AnimateFall()
    {
        tile.transform.DOMoveY(targetPosition.y, ANIMATION_DURATION)
            .SetEase(Ease.InCubic)
            .OnComplete(() => targetCell = null);
    }
}
