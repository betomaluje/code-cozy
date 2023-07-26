using System;
using UnityEngine;

public class WorldTile : MonoBehaviour
{
    [SerializeField] private TileConfig _tileConfig;
    [SerializeField] private WorldTileDeath _worldTileDeath;

    public bool IsWalkable => _tileConfig.Walkable;
    public int GetSellPrice => _tileConfig.SellProfit;

    private void Awake()
    {
        _tileConfig.Setup();
    }

    public void PerformDie()
    {
        _worldTileDeath.Die(_tileConfig.Color);
    }
}
