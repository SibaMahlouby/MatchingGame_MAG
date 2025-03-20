//It represents a cube tile 

using UnityEngine;


public class CubeTile : Tiles
{
    private MatchType matchType;

    public void PrepareCubeTile(TileBase tileBase, MatchType matchType)
    {
        SoundID = SoundID.Cube;
        this.matchType = matchType;
        tileBase.Clickable = true;
        Prepare(tileBase, GetSpritesForMatchType());
    }
    private Sprite GetSpritesForMatchType()
    {
        var imageLibrary = TilesImageLibrary.Instance;
        switch (matchType)
        {
            case MatchType.Green:
                return imageLibrary.GreenCubeSprite;
            case MatchType.Yellow:
                return imageLibrary.YellowCubeSprite;
            case MatchType.Blue:
                return imageLibrary.BlueCubeSprite;
            case MatchType.Red:
                return imageLibrary.RedCubeSprite;
        }
        return null;
    }
    public override MatchType GetMatchType()
    {
        return matchType;
    }
   

    public override void TryExecute()
    {
        ScoreManager.Instance.IncreaseScore(5); // Increase score by 10
        AudioManager.Instance.PlayEffect(SoundID);
        base.TryExecute();
    }
}