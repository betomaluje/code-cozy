using UnityEngine;

public class VillagerDeath : MonoBehaviour
{
    [SerializeField] private Transform _deathPrefab;
    [SerializeField] private Transform _deathParticles;
    [SerializeField] private int _moneyWhenDead = 2;

    public int AmountPerVillager => _moneyWhenDead;

    public void Die(GameObject villager)
    {
        var deadVillager = Instantiate(_deathPrefab, villager.transform.position, Quaternion.identity);
        if (_deathParticles != null)
        {
            Instantiate(_deathParticles, villager.transform.position, Quaternion.identity);
        }
        PopulationManager.Instance.Remove(1);
        Destroy(villager);
        Destroy(deadVillager.gameObject, 1.5f);
    }
}