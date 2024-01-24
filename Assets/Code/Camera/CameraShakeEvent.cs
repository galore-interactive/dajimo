using UnityEngine;

public class CameraShakeEvent
{
    private readonly CameraShakeConfigurationSO _configuration;
    private float _timeRemaining;

    private Vector3 _shakePositionValuesInPerlinNoiseMap;
    private Vector3 _shakePerlinNoiseValues;

    private Vector3 _rotationalShake;
    private Vector3 _translationalShake;

    public Vector3 RotationalShake => _rotationalShake;
    public Vector3 TranslationalShake => _translationalShake;

    public CameraShakeType Type => _configuration.Type;

    public CameraShakeEvent(CameraShakeConfigurationSO configuration)
    {
        _configuration = configuration;
        _timeRemaining = _configuration.Duration;

        InitShakeValuesPositionInPerlinNoiseMap();
    }

    private void InitShakeValuesPositionInPerlinNoiseMap()
    {
        float random = 32f;
        _shakePositionValuesInPerlinNoiseMap.x = Random.Range(0f, random);
        _shakePositionValuesInPerlinNoiseMap.y = Random.Range(0f, random);
        _shakePositionValuesInPerlinNoiseMap.z = Random.Range(0f, random);
    }

    public void Update()
    {
        ResetShakingVariables();

        _timeRemaining -= Time.deltaTime;

        GetNextShakeValuesPositionInPerlinNoiseMap();
        _shakePerlinNoiseValues = GetPerlinNoiseValuesFromPosition();

        if (_configuration.Type.CompareTo(CameraShakeType.Translational) == 0)
        {
            UpdateTranslationalShake();
        }
        else if (_configuration.Type.CompareTo(CameraShakeType.Rotational) == 0)
        {
            UpdateRotationalShake();
        }
        else
        {
            UpdateTranslationalShake();
            UpdateRotationalShake();
        }
    }

    private void ResetShakingVariables()
    {
        _rotationalShake = Vector3.zero;
        _translationalShake = Vector3.zero;
    }

    private void GetNextShakeValuesPositionInPerlinNoiseMap()
    {
        float displacement = Time.deltaTime * _configuration.Frequency;

        _shakePositionValuesInPerlinNoiseMap.x += displacement;
        _shakePositionValuesInPerlinNoiseMap.y += displacement;
        _shakePositionValuesInPerlinNoiseMap.z += displacement;
    }

    private void UpdateRotationalShake()
    {
        _rotationalShake = _shakePerlinNoiseValues;
        _rotationalShake -= Vector3.one * 0.5f; //From (0, 1) to (-0.5, 0.5)

        Vector3 rotationalAmplitudeVector = GetRotationalAmplitudeVector();
        ApplyRotationalAmplitudeToRotationalShake(rotationalAmplitudeVector);

        _rotationalShake *= GetShakeCurveValueInCurrentLifeTimePosition();
    }

    private Vector3 GetPerlinNoiseValuesFromPosition()
    {
        return new Vector3(
            Mathf.PerlinNoise(_shakePositionValuesInPerlinNoiseMap.x, 0f),
            Mathf.PerlinNoise(_shakePositionValuesInPerlinNoiseMap.y, 1f),
            Mathf.PerlinNoise(_shakePositionValuesInPerlinNoiseMap.z, 2f));
    }

    private Vector3 GetRotationalAmplitudeVector()
    {
        return new Vector3(_configuration.RotationalAmplitudeInX, _configuration.RotationalAmplitudeInY, _configuration.RotationalAmplitudeInZ);
    }

    private void ApplyRotationalAmplitudeToRotationalShake(Vector3 amplitudeToApply)
    {
        _rotationalShake.x *= amplitudeToApply.x;
        _rotationalShake.y *= amplitudeToApply.y;
        _rotationalShake.z *= amplitudeToApply.z;
    }

    private float GetShakeCurveValueInCurrentLifeTimePosition()
    {
        float lifeTimePercent = 1f - (_timeRemaining / _configuration.Duration);
        return _configuration.ShakeOverLifeTime.Evaluate(lifeTimePercent);
    }

    private void UpdateTranslationalShake()
    {
        _translationalShake = _shakePerlinNoiseValues;
        _translationalShake -= Vector3.one * 0.5f; //From (0, 1) to (-0.5, 0.5)

        Vector3 translationalAmplitudeVector = GetTranslationalAmplitudeVector();
        ApplyTranslationalAmplitudeToTranslationalShake(translationalAmplitudeVector);

        _translationalShake *= GetShakeCurveValueInCurrentLifeTimePosition();
    }

    private Vector3 GetTranslationalAmplitudeVector()
    {
        return new Vector3(_configuration.TranslationalAmplitudeInX, _configuration.TranslationalAmplitudeInY, _configuration.TranslationalAmplitudeInZ);
    }

    private void ApplyTranslationalAmplitudeToTranslationalShake(Vector3 amplitudeToApply)
    {
        _translationalShake.x *= amplitudeToApply.x;
        _translationalShake.y *= amplitudeToApply.y;
        _translationalShake.z *= amplitudeToApply.z;
    }

    public bool IsAlive()
    {
        return _timeRemaining > 0f;
    }
}
