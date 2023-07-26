using UnityEngine;

public class WorldTileDeath : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _deathParticles;

    private readonly int ANIM_DIE = Animator.StringToHash("World_Tile_Death");
    
    public void Die(Color tileColor)
    {
        CreateParticles(tileColor);
        _animator.CrossFade(ANIM_DIE, 0);
        Destroy(gameObject, .35f);
    }

    private void CreateParticles(Color color)
    {
        var particles = Instantiate(_deathParticles, transform.position, Quaternion.identity);

        foreach (var p in particles.GetComponentsInChildren<ParticleSystem>())
        {
            var main = p.main;
            main.startColor = color;
        }
    }
}