using UnityEngine;

public class ParticleManager : Singleton<ParticleManager>
{
    public ParticleSystem CubeParticleBlue;
    public ParticleSystem CubeParticleRed;
    public ParticleSystem CubeParticleYellow;
    public ParticleSystem CubeParticleGreen;
    //public ParticleSystem ComboHintParticle;
    public ParticleSystem BoxParticle;


    public void PlayParticle(Tiles tile)
    {
        ParticleSystem particleSystemReference;
        switch (tile.tileType)
        {
            case TileType.GreenCube:
                particleSystemReference = CubeParticleGreen;
                break;
            case TileType.BlueCube:
                particleSystemReference = CubeParticleBlue;
                break;
            case TileType.RedCube:
                particleSystemReference = CubeParticleRed;
                break;
            case TileType.YellowCube:
                particleSystemReference = CubeParticleYellow;
                break;
            case TileType.Box:
                particleSystemReference = BoxParticle;
                break;
            default:
                return;
        }
        Vector3 spawnPosition = new Vector3(tile.transform.position.x, tile.transform.position.y, -10);
        var particle = Instantiate(particleSystemReference, spawnPosition, Quaternion.identity, tile.Cell.gameGrid.particlesParent);

        particle.Play();
    }

  /*  public ParticleSystem PlayComboParticleOnTile(Tiles tile)
    {
        var particle = Instantiate(ComboHintParticle, tile.transform.position, Quaternion.identity, tile.transform);
        particle.Play(true);
        return particle;
    }*/
    public void CurrentItemParticleDestroyer(Tiles tile)
    {
        if (tile.Particle != null)
        {
            Destroy(tile.Particle.gameObject);
        }
    }

}
