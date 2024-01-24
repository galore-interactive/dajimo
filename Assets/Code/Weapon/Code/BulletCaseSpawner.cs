using UnityEngine;

public class BulletCaseSpawner
{
    private readonly ParticleSystem _particleSystem;

    public BulletCaseSpawner(ParticleSystem particleSystem)
    {
        _particleSystem = particleSystem;
    }

    public void Spawn()
    {
        _particleSystem.Emit(1);
    }
}
