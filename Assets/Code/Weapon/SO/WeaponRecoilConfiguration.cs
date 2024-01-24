using UnityEngine;

[System.Serializable]
public class WeaponRecoilConfiguration
{
    [Header("Main settings")]
    [SerializeField] private float _rotationReturnSpeed;
    [SerializeField] private float _rotationalRecoilSpeed;

    [Header("Recoil settings when not aiming")]
    [SerializeField] private Vector3 _recoilRotation;

    [Header("Recoil settings when aiming")]
    [SerializeField] private Vector3 _recoilAimingRotation;

    public float RotationReturnSpeed => _rotationReturnSpeed;
    public float RotationalRecoilSpeed => _rotationalRecoilSpeed;
    public Vector3 RecoilRotation => _recoilRotation;
    public Vector3 RecoilAimingRotation => _recoilAimingRotation;

    public WeaponRecoilConfiguration(){ }

    public WeaponRecoilConfiguration(float rotationReturnSpeed, float rotationalRecoilSpeed, Vector3 recoilRotation, Vector3 recoilAimingRotation)
    {
        _rotationReturnSpeed = rotationReturnSpeed;
        _rotationalRecoilSpeed = rotationalRecoilSpeed;
        _recoilRotation = recoilRotation;
        _recoilAimingRotation = recoilAimingRotation;
    }
}
