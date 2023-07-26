using System;
using UnityEngine;

public class PopulationManager : SingletonMono<PopulationManager>
{
    [SerializeField] private Transform _villagersContainer;

    public Transform VillagersContainer => _villagersContainer;
    public Action<int> OnPopulationChange = delegate {  };
    public Action<int> OnPopulationReduced = delegate {  };
    public Action<int> OnPopulationIncreased = delegate {  };
    
    private int _currentPopulation;

    public void Add(int amount)
    {
        if (amount <= 0) return;

        _currentPopulation += amount;

        OnPopulationChange?.Invoke(_currentPopulation);
        OnPopulationIncreased?.Invoke(amount);
    }

    public void Remove(int amount)
    {
        if (amount <= 0) return;

        _currentPopulation -= amount;
        
        if (_currentPopulation <= 0)
            _currentPopulation = 0;
        
        OnPopulationChange?.Invoke(_currentPopulation);
        OnPopulationReduced?.Invoke(amount);
    }
}