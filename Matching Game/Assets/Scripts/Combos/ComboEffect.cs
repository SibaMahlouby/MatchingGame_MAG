using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
public abstract class ComboEffect : ScriptableObject
{
    public abstract void ApplyEffect(Cell cell, List<Cell> matchedCells);
    public abstract List<Cell> GetAffectedCells(Cell cell);

    public float mergeAnimationTime = 0.3f;
    public Tween mergeTween;

    protected virtual void CreateMergeAnimationForMatchedCells(Cell cell, List<Cell> matchedCells)
    {
        cell.tile.SpriteRenderer.sortingOrder += 10;

        foreach (var matchedCell in matchedCells)
        {
            if (matchedCell.tile == cell) return;

            mergeTween = matchedCell.tile.transform.DOMove(cell.transform.position, mergeAnimationTime);
        }
    }
    protected virtual void ExecuteTilesInAffectedCells(Cell cell)
    {
        List<Cell> affectedCells = GetAffectedCells(cell);
        foreach (Cell affectedCell in affectedCells)
        {
            if (affectedCell.tile == null) continue;

            affectedCell.tile.TryExecute();
        }
    }
    protected virtual void RemoveTileFromMatchedCells(List<Cell> matchedCells)
    {
        foreach (var matchedCell in matchedCells)
        {
            if (matchedCell.tile == null) continue;
            matchedCell.tile.RemoveTile();
        }
    }
    protected void PrepareCellForAnimation(Cell cell)
    {
        cell.tile.IsFallable = false;
        cell.tile.SpriteRenderer.enabled = false;
    }
}