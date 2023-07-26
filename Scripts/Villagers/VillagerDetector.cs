using UnityEngine;

public class VillagerDetector : MonoBehaviour
{
    [SerializeField] private LayerMask _blockMask;
    
    [Header("Fishing")] 
    [SerializeField] private Transform _fishingPoint;
    [SerializeField] private float _fishingRadius = 2;
    [SerializeField] private LayerMask _waterMask;

    public Vector2 GetFishingRodStart()
    {
        var worldPosition = transform.position;
        // we need to add this offset so it looks that it comes from the middle of the player
        worldPosition.y += .75f;
        return worldPosition;
    }

    private Vector2 _lastDirection = Vector2.right;

    public bool CanMove(Vector2 destination)
    {
        _lastDirection = destination;

        var hit = Physics2D.OverlapCircle(destination,
            .2f, _blockMask);

        return hit && hit.transform.TryGetComponent<WorldTile>(out var tile) && tile.IsWalkable;
    }

    public bool IsThereWater(Vector2 direction, out Vector2 hitPosition)
    {
        // two hits, up and down

        var hitDown = GetLineCast(direction + Vector2.down / 2f);
        var hitUp = GetLineCast(direction + Vector2.up / 2f);

        if (hitDown)
        {
            hitPosition = hitDown.transform.position;
            return true;
        }

        if (hitUp)
        {
            hitPosition = hitUp.transform.position;
            return true;
        }

        hitPosition = Vector2.zero;

        return false;
    }

    private RaycastHit2D GetLineCast(Vector2 direction)
    {
        Debug.DrawLine(_fishingPoint.position,
            (Vector2) _fishingPoint.position + direction * _fishingRadius,
            Color.blue
        );

        return Physics2D.Linecast(
            _fishingPoint.position,
            (Vector2) _fishingPoint.position + direction * _fishingRadius,
            _waterMask
        );
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_lastDirection, .5f);
    }
}