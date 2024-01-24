using UnityEngine;

[System.Serializable]
public class WeaponShootingConfiguration
{
    [SerializeField] private FireMode _fireMode = FireMode.Semi;
    [SerializeField] private float _fireRate = 0.1f;
    [SerializeField] private int _shotDamage = 10;
    [SerializeField] private string _shootableLayerName = "Shootable";

    public FireMode FireMode => _fireMode;
    public float FireRate => _fireRate;
    public int ShotDamage => _shotDamage;
    public string ShootableLayerName => _shootableLayerName;

    public WeaponShootingConfiguration() { }

    public WeaponShootingConfiguration(FireMode fireMode, float fireRate, int shotDamage, string shootableLayerName)
    {
        _fireMode = fireMode;
        _fireRate = fireRate;
        _shotDamage = shotDamage;
        _shootableLayerName = shootableLayerName;
    }
}
