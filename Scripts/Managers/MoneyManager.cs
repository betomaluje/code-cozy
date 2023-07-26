using System;
using TMPro;
using UnityEngine;

public class MoneyManager : SingletonMono<MoneyManager>
{
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private int _initialMoney = 30;

    public int CurrentMoney => _currentMoney;
    public Action<int> OnMoneyChanged = delegate {  };
    
    private int _currentMoney;
    
    private void Start()
    {
        _currentMoney = _initialMoney;
        
        SetMoneyText(_currentMoney);
    }

    private void SetMoneyText(int money)
    {
        _moneyText.text = $"${money}";
    }

    public bool CanMakePurchase(int amount) => _currentMoney > 0 && amount <= _currentMoney;

    public void AddMoney(int money)
    {
        _currentMoney += money;
        SoundManager.instance.Play("Money");
        OnMoneyChanged?.Invoke(_currentMoney);
        SetMoneyText(_currentMoney);
    }

    public void SubstractMoney(int money)
    {
        _currentMoney -= money;
        if (_currentMoney < 0)
            _currentMoney = 0;
        SoundManager.instance.Play("LessMoney");
        OnMoneyChanged?.Invoke(_currentMoney);
        SetMoneyText(_currentMoney);
    }
}