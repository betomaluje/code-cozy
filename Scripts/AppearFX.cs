using DG.Tweening;
using UnityEngine;

public class AppearFX : MonoBehaviour
{
    [SerializeField] private float _animDuration = .3f;
    [SerializeField] private float _initScale = .5f;
    
    private void Awake()
    {
        transform.localScale = Vector3.one * _initScale;

        transform.DOScale(Vector3.one, _animDuration);
    }
}
