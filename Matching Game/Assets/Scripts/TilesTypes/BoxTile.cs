//Represents Box tile

using System;


public class BoxTile : Tiles
{
    private const int HEALTH = 1;

    public void PrepareBoxTile(TileBase tileBase)
    {
        SoundID = SoundID.Box;
        tileBase.IsFallable = false;
        tileBase.Health = HEALTH;
        tileBase.InterectWithExplode = true;
        tileBase.Clickable = false;
        Prepare(tileBase, TilesImageLibrary.Instance.GetSpriteForTileType(tileBase.tileType));
    }

    public override void TryExecute()
    {
        //ParticleManager.Instance.PlayParticle(this);
        AudioManager.Instance.PlayEffect(SoundID);
        ScoreManager.Instance.IncreaseScore(10); // Increase score by 10
        base.TryExecute();
    }
}
