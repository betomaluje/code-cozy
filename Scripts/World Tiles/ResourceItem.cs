using UnityEngine;

public class ResourceItem : MonoBehaviour
{
    [SerializeField] private int _moneyProfit = 5;
    [SerializeField] private LayerMask _targetMask;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!_targetMask.LayerMatchesObject(col.gameObject)) return;
        
        MoneyManager.Instance.AddMoney(_moneyProfit);
        Destroy(gameObject);
    }
}
