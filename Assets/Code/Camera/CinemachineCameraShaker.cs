using UnityEngine;
using Unity.Cinemachine;

public class CinemachineCameraShaker
{
    private readonly CinemachineBasicMultiChannelPerlin _noiseComponent;
    private bool _isPerformingAShake = false;
    private float _shakeLeftTime;

    public CinemachineCameraShaker(CinemachineBasicMultiChannelPerlin noiseComponent)
    {
        _noiseComponent = noiseComponent;
    }

    public void Update()
    {
        _noiseComponent.AmplitudeGain = 0f;
        if (_isPerformingAShake)
        {
            _noiseComponent.AmplitudeGain = 1f;
            _shakeLeftTime -= Time.deltaTime;
            if (_shakeLeftTime <= 0f)
            {
                _isPerformingAShake = false;
            }
        }
    }

    public void ShakeCamera(float duration)
    {
        if (_isPerformingAShake)
        {
            return;
        }

        _isPerformingAShake = true;
        _shakeLeftTime = duration;
    }

    public void ShakeCamera(NoiseSettings shakeSettings, float duration)
    {
        if (_isPerformingAShake)
        {
            return;
        }

        _isPerformingAShake = true;
        _noiseComponent.NoiseProfile = shakeSettings;
        _shakeLeftTime = duration;
    }
}
