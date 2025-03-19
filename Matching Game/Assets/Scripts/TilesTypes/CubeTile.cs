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
    public override void HintUpdateToSprite(TileType tileType)
    {
        var imageLibrary = TilesImageLibrary.Instance;

        switch (tileType)
        {
            case TileType.TNT:
                UpdateColorfulBombSprite(imageLibrary);
                break;
            default:
                UpdateSprite(GetSpritesForMatchType());
                break;
        }
    }
    private void UpdateColorfulBombSprite(TilesImageLibrary imageLibrary)
    {
        Sprite newSprite;
        switch (matchType)
        {
            case MatchType.Green:
                newSprite = imageLibrary.GreenCubeBombHintSprite;
                break;
            case MatchType.Yellow:
                newSprite = imageLibrary.YellowCubeBombHintSprite;
                break;
            case MatchType.Blue:
                newSprite = imageLibrary.BlueCubeBombHintSprite;
                break;
            case MatchType.Red:
                newSprite = imageLibrary.RedCubeBombHintSprite;
                break;
            default:
                return;
        }
        UpdateSprite(newSprite);
    }

    public override void TryExecute()
    {
        ScoreManager.Instance.IncreaseScore(5); // Increase score by 10
        AudioManager.Instance.PlayEffect(SoundID);
        base.TryExecute();
    }
}