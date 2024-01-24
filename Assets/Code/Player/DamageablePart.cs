using UnityEngine;

public class DamageablePart : MonoBehaviour, IDamageable, INetworkEntityID
{
    private IDamageable _damageableEntity;
    private uint _networkEntityID = uint.MaxValue;

    public uint GetNetworkEntityId()
    {
        return _networkEntityID;
    }

    public void SetDamageableEntity(IDamageable dam)
    {
        _damageableEntity = dam;
    }

    public void Server_TakeDamage(int damage)
    {
        _damageableEntity.Server_TakeDamage(damage);
    }

    public void SetNetworkEntityID(uint entityID)
    {
        _networkEntityID = entityID;
    }
}
