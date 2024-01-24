using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Camera/Camera Shake Configuration", fileName = "CameraShakeConfiguration")]
public class CameraShakeConfigurationSO : ScriptableObject
{
    [SerializeField] private CameraShakeType _type = CameraShakeType.Rotational;

    [SerializeField] private float _duration = 1f;
    [SerializeField] private float _frequency = 1f;
    [Space]
    [SerializeField] private float _translationalAmplitudeInX = 1f;
    [SerializeField] private float _translationalAmplitudeInY = 1f;
    [SerializeField] private float _translationalAmplitudeInZ = 1f;
    [Space]
    [SerializeField] private float _rotationalAmplitudeInX = 1f;
    [SerializeField] private float _rotationalAmplitudeInY = 1f;
    [SerializeField] private float _rotationalAmplitudeInZ = 1f;
    [Space]
    [SerializeField] private AnimationCurve _shakeOverLifeTime = new AnimationCurve(
        new Keyframe(0f, 0f, Mathf.Deg2Rad * 0f, Mathf.Deg2Rad * 720f),
        new Keyframe(0.2f, 1f),
        new Keyframe(1f, 0f));

    public CameraShakeType Type => _type;
    public float Duration => _duration;
    public float Frequency => _frequency;

    public float TranslationalAmplitudeInX => _translationalAmplitudeInX;
    public float TranslationalAmplitudeInY => _translationalAmplitudeInY;
    public float TranslationalAmplitudeInZ => _translationalAmplitudeInZ;

    public float RotationalAmplitudeInX => _rotationalAmplitudeInX;
    public float RotationalAmplitudeInY => _rotationalAmplitudeInY;
    public float RotationalAmplitudeInZ => _rotationalAmplitudeInZ;

    public AnimationCurve ShakeOverLifeTime => _shakeOverLifeTime;
}
