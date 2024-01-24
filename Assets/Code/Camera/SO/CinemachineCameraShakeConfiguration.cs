using UnityEngine;
using Cinemachine;

[CreateAssetMenu(menuName = "Scriptable Objects/Camera/Cinemachine Shake Configuration", fileName = "CinemachineShakeConfiguration")]
public class CinemachineCameraShakeConfiguration : ScriptableObject
{
    [SerializeField] private NoiseSettings _noiseSettings;
    [SerializeField] private float _duration = 1f;
    [SerializeField]
    private AnimationCurve _shakeOverLifeTime = new AnimationCurve(
        new Keyframe(0f, 0f, Mathf.Deg2Rad * 0f, Mathf.Deg2Rad * 720f),
        new Keyframe(0.2f, 1f),
        new Keyframe(1f, 0f));

    public NoiseSettings NoiseSettings => _noiseSettings;
    public float Duration => _duration;
    public AnimationCurve ShakeOverLifeTime => _shakeOverLifeTime;
}