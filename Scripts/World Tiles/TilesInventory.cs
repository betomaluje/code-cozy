using UnityEngine;

public class TilesInventory : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    
    private TileConfig[] _allTiles;
    private int _currentIndex;

    private void Start()
    {
        _allTiles = InventoryManager.Instance.Tiles;
        _currentIndex = InventoryManager.Instance.FirstItemIndex;
    }

    private void OnEnable()
    {
        _playerInput.ChangeEvent += HandleInventoryChange;
    }

    private void OnDisable()
    {
        _playerInput.ChangeEvent -= HandleInventoryChange;
    }

    private void HandleInventoryChange()
    {
        _currentIndex++;
        if (_currentIndex > _allTiles.Length - 1)
        {
            _currentIndex = 0;
        }
    }

    public TileConfig GetTile()
    {
        return _allTiles[_currentIndex];
    }
}