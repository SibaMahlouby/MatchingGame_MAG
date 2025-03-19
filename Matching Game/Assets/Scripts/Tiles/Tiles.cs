// For representing tile in the game. It contains the tile properties and methods to prepare and control the tile.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tiles : MonoBehaviour
{
    private const int BaseSortingOrder = 10;
    private static int childSpriteOrder;
    public SpriteRenderer SpriteRenderer;

    public TileType tileType;
    public bool Clickable;
    public bool InterectWithExplode;
    public bool IsFallable;
    public int Health;

    public FallAnimation FallAnimation;
    public ParticleSystem Particle;
    private Cell cell;
    public SoundID SoundID = SoundID.None;
    public Cell Cell
    {
        get { return cell; }
        set
        {
            if (cell == value) return;

            var oldCell = cell;
            cell = value;

            if (oldCell != null && oldCell.tile == this)
                oldCell.tile = null;

            if (value != null)
            {
                value.tile = this;
                gameObject.name = cell.gameObject.name + " " + GetType().Name;
            }
        }
    }


    public void Prepare(TileBase tileBase, Sprite sprite)
    {
        SpriteRenderer = AddSprite(sprite);

        tileType = tileBase.tileType;
        Clickable = tileBase.Clickable;
        InterectWithExplode = tileBase.InterectWithExplode;
        IsFallable = tileBase.IsFallable;
        FallAnimation = tileBase.FallAnimation;
        Health = tileBase.Health;
        FallAnimation.tile = this;
    }

    public SpriteRenderer AddSprite(Sprite sprite)
    {
        var spriteRenderer = new GameObject("Sprite_" + childSpriteOrder).AddComponent<SpriteRenderer>();
        spriteRenderer.transform.SetParent(transform);
        spriteRenderer.transform.localPosition = Vector3.zero;
        spriteRenderer.transform.localScale = new Vector2(0.7f, 0.7f);
        spriteRenderer.sprite = sprite;
        spriteRenderer.sortingLayerID = SortingLayer.NameToID("Cell");
        spriteRenderer.sortingOrder = BaseSortingOrder + childSpriteOrder++;

        return spriteRenderer;
    }

    public virtual MatchType GetMatchType() { return MatchType.None; }

    public virtual void TryExecute()
    {
        GoalManager.Instance.UpdateLevelGoal(tileType);
        RemoveTile();
    }
    public void RemoveTile()
    {
        Cell.tile = null;
        Cell = null;
        Destroy(gameObject);
    }

    public void UpdateSprite(Sprite sprite)
    {
        var spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
    }

    public virtual void HintUpdateToSprite(TileType tileType)
    {
        return;
    }
    public void Fall()
    {
        if (!this.IsFallable) return;

        FallAnimation.FallTo(cell.GetFallTarget());
    }
}
