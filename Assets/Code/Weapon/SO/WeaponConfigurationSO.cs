using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Weapon/Weapon Configuration", fileName = "WeaponConfiguration")]
public class WeaponConfigurationSO : ScriptableObject
{
    [SerializeField] private WeaponId _weaponId = WeaponId.None;

    [Header("Shooting Configuration")]
    [SerializeField] private WeaponShootingConfiguration _shootingConfiguration;

    [Header("Aiming Configuration")]
    [SerializeField] private WeaponAimingConfiguration _aimingConfiguration;

    [Header("Recoil Configuration")]
    [SerializeField] private WeaponRecoilConfiguration _recoilConfiguration;

    [Header("Sway Configuration")]
    [SerializeField] private WeaponSwayConfiguration _swayConfiguration;

    [Header("Sounds Configuration")]
    [SerializeField] private WeaponSoundsConfiguration _soundsConfiguration;

    public WeaponId WeaponId => _weaponId;
    public WeaponShootingConfiguration ShootingConfiguration  => _shootingConfiguration;
    public WeaponAimingConfiguration AimingConfiguration  => _aimingConfiguration;
    public WeaponRecoilConfiguration RecoilConfiguration => _recoilConfiguration;
    public WeaponSwayConfiguration SwayConfiguration => _swayConfiguration;
    public WeaponSoundsConfiguration SoundsConfiguration => _soundsConfiguration;
}
