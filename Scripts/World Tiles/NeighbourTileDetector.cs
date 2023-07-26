using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NeighbourTileDetector : MonoBehaviour
{
    [SerializeField] private LayerMask _targetMask;
    [SerializeField] private float _detectionRadius = .6f;
    [SerializeField] private Transform _detectionPosition;

    private void Start()
    {
        // prevent raycast to itself
        Physics2D.queriesStartInColliders = false;
    }

    private void LateUpdate()
    {
        var hits = new List<RaycastHit2D>(4)
        {
            DetectNeighbour(Vector2.up),
            DetectNeighbour(Vector2.down),
            DetectNeighbour(Vector2.left),
            DetectNeighbour(Vector2.right)
        };

        foreach (var hit in hits.Where(hit => hit))
        {
            if (hit.collider.TryGetComponent<WorldTile>(out var worldTile))
            {
                // do something for each tile type
                // Debug.Log($"{hit.transform.name} -> Tile type {worldTile.TileType.ToString()}");
            }
        }
    }

    private RaycastHit2D DetectNeighbour(Vector2 direction)
    {
        Vector2 currentPosition = _detectionPosition.position;

        var hit = Physics2D.Raycast(
            currentPosition,
            direction,
            _detectionRadius,
            _targetMask);

        return hit;
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 currentPosition = _detectionPosition.position;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(currentPosition, Vector2.up * _detectionRadius);
        Gizmos.DrawRay(currentPosition, Vector2.down * _detectionRadius);
        Gizmos.DrawRay(currentPosition, Vector2.left * _detectionRadius);
        Gizmos.DrawRay(currentPosition, Vector2.right * _detectionRadius);
    }
}