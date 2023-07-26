using UnityEngine;

public class InventoryManager : SingletonMono<InventoryManager>
{
    [SerializeField] private bool _isDebug;
    
    private TileConfig[] _allTiles;

    public TileConfig[] Tiles => _allTiles;
    public int FirstItemIndex = 1;

    private void Start()
    {
        _allTiles = Resources.LoadAll<TileConfig>(_isDebug ? "DEMO" : "Tiles");
    }
}