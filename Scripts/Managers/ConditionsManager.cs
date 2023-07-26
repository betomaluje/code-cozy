using System;
using System.Linq;
using UnityEngine;

public class ConditionsManager : MonoBehaviour
{
    public Action OnGameOver = delegate {  };
    
    private int _currentTiles;
    private TileConfig[] _allTiles;

    private void Start()
    {
        Invoke(nameof(StartConditions), 1f);
    }

    private void StartConditions()
    {
        // we start with one tile
        _currentTiles = FindObjectsOfType<WorldTile>().Length;
        _allTiles = InventoryManager.Instance.Tiles;
    }

    private void OnEnable()
    {
        MoneyManager.Instance.OnMoneyChanged += HandleMoneyChanged;
    }

    private void OnDisable()
    {
        MoneyManager.Instance.OnMoneyChanged -= HandleMoneyChanged;
    }

    private void HandleMoneyChanged(int currentAmount)
    {
        CheckGameConditions();
    }

    public void OnTileAdded()
    {
        _currentTiles++;
    }

    public void OnTileRemoved()
    {
        _currentTiles--;
        if (_currentTiles < 0)
            _currentTiles = 0;

        CheckGameConditions();
    }

    private void CheckGameConditions()
    {
        if (_allTiles == null) return;
        var currentMoney = MoneyManager.Instance.CurrentMoney;
        var hasMoneyForAnyTile = _allTiles.Any(tile => currentMoney >= tile.PurchaseCost);
        var isGameOver = _currentTiles <= 0 && !hasMoneyForAnyTile;

        if (!isGameOver) return;
        
        OnGameOver?.Invoke();
        Time.timeScale = 0;
    }
}