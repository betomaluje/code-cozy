using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileSpawnExtras : MonoBehaviour
{
    [SerializeField] private float _initialSpawnTime = .5f;
    [SerializeField] private float _positionRange = 1f;

    [Header("Population")] 
    [SerializeField] private int _maxPopulation = 2;

    [SerializeField] private Transform[] _availablePeople;

    [Space] 
    [Header("Resources")]
    [SerializeField] private int _maxResources = 2;
    [SerializeField] private Transform[] _resourcePrefabs;
    [Tooltip("Chance of spawning a resource, if any. The higher, the more chances")]
    [SerializeField, Range(0, 1)] private float _resourceSpawnChance = .5f;
    
    [Tooltip("Chance of spawning a resource instead of population. The higher, the more chances")]
    [SerializeField, Range(0, 1)] private float _spawnChance = .5f;

    private void Start()
    {
        Invoke(nameof(DecideSpawn), _initialSpawnTime);
    }

    private void DecideSpawn()
    {
        if (_spawnChance >= Random.Range(0f, 1f))
        {
            SpawnResources();
        }
        else
        {
            SpawnPopulation();
        }
    }

    private void SpawnResources()
    {
        var resources = Random.Range(0, _maxResources + 1);
        for (var i = 0; i < resources; i++)
        {
            if (!(_resourceSpawnChance >= Random.Range(0f, 1f))) continue;
            
            var index = Random.Range(0, _resourcePrefabs.Length);
            var resource = Spawn(_resourcePrefabs[index]);
            resource.name = $"Resource {i + 1}";
            resource.parent = transform;
        }
    }

    private void SpawnPopulation()
    {
        var population = Random.Range(0, _maxPopulation + 1);
        for (var i = 0; i < population; i++)
        {
            var index = Random.Range(0, _availablePeople.Length);
            var villager = Spawn(_availablePeople[index]);
            villager.name = $"Villager {i + 1}";
            villager.parent = PopulationManager.Instance.VillagersContainer;
        }
        
        PopulationManager.Instance.Add(population);
    }

    private Transform Spawn(Transform prefab)
    {
        var randomPositionRange = Random.Range(-_positionRange, _positionRange);
        var random = Random.insideUnitCircle + new Vector2(randomPositionRange, randomPositionRange) ;
        return Instantiate(
            prefab,
            (Vector2) transform.position + random,
            Quaternion.identity
        );
    }
}