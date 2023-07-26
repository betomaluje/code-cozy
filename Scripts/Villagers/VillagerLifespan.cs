using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VillagerLifespan : MonoBehaviour
{
    [SerializeField] private float _maxLifespan = 100;
    [SerializeField] private float _startingLifespan = 100;
    [SerializeField] private float _sliderAnimDuration = .3f;
    [SerializeField] private float _lifespanDecreaseRate = .1f;
    [SerializeField] private Slider _slider;

    private float _currentLifespan;
    private bool _isUpdating;

    private void Start()
    {
        _currentLifespan = _startingLifespan;
        StartCoroutine(UpdateUI());
    }

    private IEnumerator UpdateUI()
    {
        _isUpdating = true;

        var endValue = _currentLifespan / _maxLifespan;
        var startValue = _slider.value;

        var time = 0f;
        var duration = _sliderAnimDuration;

        while (time < duration)
        {
            _slider.value = Mathf.Lerp(startValue, endValue, time / duration);

            time += Time.deltaTime;

            yield return null;
        }

        _slider.value = endValue;

        _isUpdating = false;  
    }

    private void LateUpdate()
    {
        if (_isUpdating) return;
        
        if (_currentLifespan <= 0)
        {
            _isUpdating = true;
            Die();
            return;
        }

        _currentLifespan = Mathf.Max(0, _currentLifespan - _lifespanDecreaseRate * Time.deltaTime);

        _slider.value = Mathf.Lerp(_slider.value, _currentLifespan / _maxLifespan, Time.deltaTime);
    }

    private void Die()
    {
        if (!TryGetComponent<VillagerDeath>(out var villagerDeath)) return;

        MoneyManager.Instance.AddMoney(villagerDeath.AmountPerVillager);
        villagerDeath.Die(gameObject);
    }
}