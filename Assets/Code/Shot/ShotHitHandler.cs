using UnityEngine;

public class ShotHitHandler : MonoBehaviour
{
    private RaycastShooter _raycastShooter;

    private BulletHoleSpawner _bulletHoleSpawner;
    private BulletImpactSpawner _bulletImpactSpawner;

    private void Awake()
    {
        _raycastShooter = FindObjectOfType<RaycastShooter>();
        _bulletHoleSpawner = FindObjectOfType<BulletHoleSpawner>();
        _bulletImpactSpawner = FindObjectOfType<BulletImpactSpawner>();
    }

    private void OnEnable()
    {
        _raycastShooter.OnShotHit += SpawnBulletHole;
        _raycastShooter.OnShotHit += SpawnBulletImpact;
    }

    private void OnDisable()
    {
        _raycastShooter.OnShotHit -= SpawnBulletHole;
        _raycastShooter.OnShotHit -= SpawnBulletImpact;
    }

    private void SpawnBulletHole(ShotHitData hitData)
    {
        _bulletHoleSpawner.Client_Spawn(hitData);
    }

    private void SpawnBulletImpact(ShotHitData hitData)
    {
        _bulletImpactSpawner.Client_Spawn(hitData);
    }
}
