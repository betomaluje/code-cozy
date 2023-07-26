using UnityEngine;

[CreateAssetMenu(fileName = "New World Tile", menuName = "World Tiles/New Tile", order = 0)]
public class TileConfig : ScriptableObject
{
    public Sprite[] TileSprites;
    public Color Color;
    public Transform Prefab;
    public bool Walkable;
    public int PurchaseCost;
    public int SellProfit = 1;

    public void Setup()
    {
        if (Prefab == null) return;

        if (!Prefab.TryGetComponentInChildren<SpriteRenderer>(out var spriteRenderer)) return;
        
        spriteRenderer.sprite = GetSprite();
    }

    public Sprite GetSprite() => TileSprites is {Length: > 0} ? TileSprites[Random.Range(0, TileSprites.Length)] : null;
}