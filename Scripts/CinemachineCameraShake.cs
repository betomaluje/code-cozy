using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CinemachineCameraShake : SingletonMono<CinemachineCameraShake>
{
    [SerializeField] private float intensity = 1.5f;
    [SerializeField] private float shakeTime = .1f;

    private CinemachineBasicMultiChannelPerlin _channelPerlin;
    private float _shakeTime;
    private float _shakeTimeTotal;

    private void Awake()
    {
        CinemachineVirtualCamera virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _channelPerlin =
            virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _shakeTimeTotal = shakeTime;
    }

    public void ShakeCamera(float intensity, float shakeTime)
    {
        this.intensity = intensity;
        this.shakeTime = shakeTime;
        ShakeCamera();
    }

    public void ShakeCamera()
    {
        _channelPerlin.m_AmplitudeGain = intensity;
        _shakeTime = shakeTime;
    }

    private void Update()
    {
        if (_shakeTime > 0)
        {
            _shakeTime -= Time.deltaTime;
        }

        _channelPerlin.m_AmplitudeGain = Mathf.Lerp(intensity, 0, 1 - (_shakeTime / _shakeTimeTotal));
    }

}
