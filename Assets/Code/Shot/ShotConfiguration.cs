using GONet;
using System;

public struct ShotConfiguration
{
    public GONetParticipant shooterGNP;
    public int damage;
    public float maxDistance;
    public WeaponId weaponId;

    public ShotConfiguration(GONetParticipant shooterGNP, int damage, float maxDistance, WeaponId weaponId)
    {
        if (!shooterGNP) throw new ArgumentNullException(nameof(shooterGNP));

        this.shooterGNP = shooterGNP;
        this.damage = damage;
        this.maxDistance = maxDistance;
        this.weaponId = weaponId;
    }
}
