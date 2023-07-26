using System;
using BerserkPixel.Debug;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class ClickAndPlace : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private TilesInventory _tilesInventory;
    [SerializeField] private float _movementFactor = 5f;
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private LayerMask _worldMask;
    [SerializeField] private LayerMask _villagersMask;
    [SerializeField] private float _checkWorldRadius = .6f;

    [Header("FXs")] 
    [SerializeField] private PopupText _tilePopupPrefab;
    [SerializeField] private PopupText _villagerPopupPrefab;

    private ConditionsManager _conditionsManager;
    private Vector2 _tileCenter;
    private Collider2D _collider;
    private bool _isZoomedIn;

    private void Awake()
    {
        _conditionsManager = FindObjectOfType<ConditionsManager>();
    }

    private void Start()
    {
        _tileCenter = GetTileCenter(transform.position);
        Invoke(nameof(PlaceFirstTile), .5f);
    }

    private void PlaceFirstTile()
    {
        var tile = _tilesInventory.GetTile();
        SoundManager.instance.PlayWithPitch("Place", Random.Range(.8f, 1.5f));

        MoneyManager.Instance.SubstractMoney(tile.PurchaseCost);
        Instantiate(tile.Prefab, _tileCenter, Quaternion.identity);
    }

    private void OnEnable()
    {
        _playerInput.MovementEvent += HandlePlayerMove;
        _playerInput.PlaceEvent += HandlePlace;
        _playerInput.RemoveEvent += HandleRemove;
    }

    private void OnDisable()
    {
        _playerInput.MovementEvent -= HandlePlayerMove;
        _playerInput.PlaceEvent -= HandlePlace;
        _playerInput.RemoveEvent -= HandleRemove;
    }

    private void HandlePlayerMove(Vector2 movement)
    {
        if (movement == Vector2.zero) return;

        var newPosition = (Vector2) transform.position + (movement * _movementFactor);

        transform.position = newPosition;
        SoundManager.instance.PlayWithPitch("Move", Random.Range(.9f, 1.1f));
        _tileCenter = GetTileCenter(transform.position);
    }

    private void HandlePlace()
    {
        PerformOverlapBox(_tileCenter, _worldMask, (raycastCollider) =>
        {
            _collider = raycastCollider;

            if (raycastCollider) return;
            
            var tile = _tilesInventory.GetTile();

            if (!MoneyManager.Instance.CanMakePurchase(tile.PurchaseCost)) return;

            SoundManager.instance.PlayWithPitch("Place", Random.Range(.8f, 1.5f));

            _conditionsManager.OnTileAdded();
            MoneyManager.Instance.SubstractMoney(tile.PurchaseCost);
            PopupTextSpawner.Instance.PopupNegativeText($"-{tile.PurchaseCost}", _tileCenter);
            Instantiate(tile.Prefab, _tileCenter, Quaternion.identity);
        });
    }

    private void HandleRemove()
    {
        var tileCenter = _tileCenter;
        
        // tiles
        PerformOverlapBox(tileCenter, _worldMask, raycastCollider =>
        {
            _collider = raycastCollider;

            if (!raycastCollider) return;

            // last destroy terrain
            if (!raycastCollider.TryGetComponent<WorldTile>(out var worldTile)) return;

            CinemachineCameraShake.Instance.ShakeCamera();
            SoundManager.instance.Play("Remove");
            
            PopupTextSpawner.Instance.PopupText(
                _tilePopupPrefab,
                $"+{worldTile.GetSellPrice}",
                tileCenter);
            MoneyManager.Instance.AddMoney(worldTile.GetSellPrice);
            _conditionsManager.OnTileRemoved();
            worldTile.PerformDie();
        });

        // villagers in tile
        PerformOverlapBoxAll(tileCenter, _villagersMask, (colliders) =>
        {
            // first kill villagers, if any
            var killed = 0;
            var totalMoney = 0;
            foreach (var col in colliders)
            {
                if (!col) continue;

                if (!col.TryGetComponent<VillagerDeath>(out var villagerDeath)) continue;

                MoneyManager.Instance.AddMoney(villagerDeath.AmountPerVillager);
                villagerDeath.Die(col.gameObject);
                killed++;
                totalMoney += villagerDeath.AmountPerVillager;
            }
            
            if (killed <= 0) return;

            PopupTextSpawner.Instance.PopupTextDelayed(
                _villagerPopupPrefab,
                $"+{totalMoney}",
                tileCenter,
                .5f);
        });
    }

    private void PerformOverlapBoxAll(Vector2 position, LayerMask layerMask, Action<Collider2D[]> callback)
    {
        var colliders = Physics2D.OverlapBoxAll(position, Vector2.one * _checkWorldRadius, 0, layerMask);
        
        Draw.DebugRect(position, _checkWorldRadius, Color.blue, 2);

        callback?.Invoke(colliders);
    }

    private void PerformOverlapBox(Vector2 position, LayerMask layerMask, Action<Collider2D> callback)
    {
        var box = Physics2D.OverlapBox(position, Vector2.one * _checkWorldRadius, 0, layerMask);

        callback?.Invoke(box);
    }

    /// <summary>
    ///     Transforms a world position (ex. a mouse position) to the center of a tile
    /// </summary>
    /// <param name="worldPosition">The <see cref="Vector3" /> reference to check</param>
    /// <returns>The center in world coordinates of a tile</returns>
    private Vector3 GetTileCenter(Vector3 worldPosition)
    {
        var tilePos = _tilemap.WorldToCell(worldPosition);
        var tileCenter = _tilemap.GetCellCenterWorld(tilePos);
        return tileCenter;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _collider ? Color.green : Color.red;
        Gizmos.DrawWireCube(_tileCenter, Vector2.one * _checkWorldRadius);
    }
}