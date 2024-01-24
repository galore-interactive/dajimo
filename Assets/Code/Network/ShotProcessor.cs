using System;
using GONet;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(NetworkController))]
public class ShotProcessor : GONetParticipantCompanionBehaviour
{
    const string METAL = "Metal";

    private NetworkController _networkController;
    private BulletHoleSpawner _bulletHoleSpawner;
    private BulletImpactSpawner _bulletImpactSpawner;

    private static GONetParticipant myASSuMEdPlayerGNP = null;

    public static void SetMyLocalPlayerGNP(GONetParticipant gONetParticipant)
    {
        myASSuMEdPlayerGNP = gONetParticipant;
    }
    
    protected override void Awake()
    {
        base.Awake();

        _networkController = GetComponent<NetworkController>();

        _bulletHoleSpawner = FindObjectOfType<BulletHoleSpawner>();
        _bulletImpactSpawner = FindObjectOfType<BulletImpactSpawner>();
    }

    public override void OnGONetParticipantStarted()
    {
        base.OnGONetParticipantStarted();

        EventBus.Subscribe<LocalContextEntityEvent>(ReceiveEntityEvent);
    }

    private void ReceiveEntityEvent(GONetEventEnvelope<LocalContextEntityEvent> entityEventEnvelope)
    {
        if (!IsClient || !myASSuMEdPlayerGNP) return;

        LocalContextEntityEvent entityEvent = entityEventEnvelope.Event;

        switch (entityEvent.type)
        {
            case EntityEventType.EV_BULLET_HIT_NON_SHOOTABLE 
            when myASSuMEdPlayerGNP.GONetId.CompareTo((uint)BitConverter.ToInt32(entityEvent.parameters, 0)) != 0:
                ShotHitData hitData = new ShotHitData(entityEvent.vector3, entityEvent.quaternion * Vector3.forward, 0, METAL, 0, null);
                _bulletHoleSpawner.Client_Spawn(hitData);
                _bulletImpactSpawner.Client_Spawn(hitData);
                break;

            case EntityEventType.EV_BULLET_HIT_SHOOTABLE:
                EventBulletHitShootable(entityEvent, myASSuMEdPlayerGNP.GONetId, myASSuMEdPlayerGNP);
                break;
        }
    }

    private void EventBulletHitShootable(LocalContextEntityEvent entityEvent, uint localGONetParticipantId, GONetParticipant localGONetParticipant)
    {
        if (localGONetParticipantId.CompareTo((uint)BitConverter.ToInt32(entityEvent.parameters, 0)) == 0)
        {
            ShowHitMarker();
        }
        else
        {
            ShotHitData hitData = new ShotHitData(entityEvent.vector3, entityEvent.quaternion * Vector3.forward, 0, METAL, 0, null);
            _bulletHoleSpawner.Client_Spawn(hitData);
            _bulletImpactSpawner.Client_Spawn(hitData);
        }

        if (localGONetParticipantId.CompareTo((uint)BitConverter.ToInt32(entityEvent.parameters, 4)) == 0)
        {
            AddDamageIndicatorEvent(entityEvent.quaternion * Vector3.forward);

            if (localGONetParticipant.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.Server_TakeDamage(10);
            }
        }
    }

    private void ShowHitMarker()
    {
        HitMarker hitmarker = FindObjectOfType<HitMarker>();
        if (hitmarker != null)
        {
            hitmarker.ShowHitMarker();
        }
    }

    private void AddDamageIndicatorEvent(Vector3 direction)
    {
        DamageIndicatorController damageIndicatorController = FindObjectOfType<DamageIndicatorController>();
        if (damageIndicatorController != null)
        {
            damageIndicatorController.Add(direction);
        }
    }

    public EntityState GetEntityStateWithoutEvent()
    {
        EntityState entityStateToReturn = new EntityState();
        entityStateToReturn.networkObjectID = GetNetworkEntityId();
        return entityStateToReturn;
    }

    public uint GetNetworkEntityId()
    {
        return GONetParticipant.GONetId;
    }
}
