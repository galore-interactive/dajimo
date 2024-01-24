using UnityEngine;
using UnityEngine.Assertions;

public class DamageSender
{
    private readonly int _damageableLayer;

    public DamageSender(int damageableLayer)
    {
        _damageableLayer = damageableLayer;
    }

    public void TrySendDamageToHitGameObject(GameObject hitGameObject, int damage, int layer)
    {
        if (!IsTargetLayerDamageable(layer))
        {
            return;
        }

        SendDamageToHitGameObject(hitGameObject, damage);
    }

    private bool IsTargetLayerDamageable(int layer)
    {
        if (layer == _damageableLayer)
        {
            return true;
        }

        return false;
    }

    private void SendDamageToHitGameObject(GameObject hitGameObject, int damage)
    {
        IDamageable damageComponent = hitGameObject.GetComponent<IDamageable>();
        Assert.IsNotNull(damageComponent, "[DamageSender at SendDamageToHitGameObject]: Could not find the IDamageable component in this GameObject.");

        damageComponent.Server_TakeDamage(damage);
    }
}
