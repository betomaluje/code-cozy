using Cinemachine;
using UnityEngine;

[DisallowMultipleComponent]
public class CameraZoom : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    [SerializeField] private float maxZoom = 20;
    [SerializeField] private float minZoom = 65;
    [SerializeField] private float speed = 60;

    private float _targetZoom;

    private void Awake()
    {
        if (virtualCamera != null)
            _targetZoom = virtualCamera.m_Lens.OrthographicSize;
    }

    private void OnEnable()
    {
        _playerInput.ZoomInEvent += SetMinimumZoom;
        _playerInput.ZoomOutEvent += SetMaximumZoom;
    }

    private void OnDisable()
    {
        _playerInput.ZoomInEvent -= SetMinimumZoom;
        _playerInput.ZoomOutEvent -= SetMaximumZoom;
    }

    private void SetMinimumZoom()
    {
        SetZoom(minZoom);
    }

    private void SetMaximumZoom()
    {
        SetZoom(maxZoom);
    }

    private void SetZoom(float zoomAmount)
    {
        _targetZoom = Mathf.Clamp(zoomAmount, minZoom, maxZoom);
    }

    private void Update()
    {
        if (virtualCamera == null) return;

        virtualCamera.m_Lens.OrthographicSize = Mathf.MoveTowards(
            virtualCamera.m_Lens.OrthographicSize,
            _targetZoom,
            speed * Time.deltaTime
        );
    }
}