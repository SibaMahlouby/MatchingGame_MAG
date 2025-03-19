//TNT tile in the game

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNTTile : Tiles
{
    private readonly MatchType matchType = MatchType.Special;

    public void PrepareTNTTile(TileBase tileBase)
    {
        SoundID = SoundID.TNT;
        var bombSprite = TilesImageLibrary.Instance.TNTSprite;
        Prepare(tileBase, bombSprite);
    }

    public override MatchType GetMatchType()
    {
        return matchType;
    }

    public override void TryExecute()
    {
        var explodeCellArea = Cell.allArea;

        AudioManager.Instance.PlayEffect(SoundID);
        base.TryExecute();

        for (int i = 0; i < explodeCellArea.Count; i++)
        {
            var currentCell = explodeCellArea[i];
            if (currentCell.tile == null) continue;
            var tile = currentCell.tile;
            tile.TryExecute();
        }
    }
}
