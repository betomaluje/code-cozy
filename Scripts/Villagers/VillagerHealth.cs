using UnityEngine;

public class VillagerHealth : MonoBehaviour
{
    [SerializeField] private int _moneyAmount = 1;
    [SerializeField] private float _timeBetween = 8f;
    
    private float _currentMoneyTime;
    private Vector2 _popupPosition;
    
    private void Start()
    {
        _currentMoneyTime = _timeBetween;
        _popupPosition = transform.position;
        _popupPosition.y += 1;
    }

    private void Update()
    {
        if (_currentMoneyTime <= 0)
        {
            MoneyManager.Instance.AddMoney(_moneyAmount);
            PopupTextSpawner.Instance.PopupText($"+{_moneyAmount}", _popupPosition);
            _currentMoneyTime = _timeBetween;
        }
        else
        {
            _currentMoneyTime -= Time.deltaTime;
        }
    }
}