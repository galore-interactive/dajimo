using UnityEngine;
using GONet;
using System;

/// <summary>
/// This class is responsible for syncing the entity state of a remote entity within a client.
/// </summary>
[Obsolete("Going pure GONet now.")]
public class EntityStateSynchronizer : GONetParticipantCompanionBehaviour
{
    private INetworkEntity _targetNetworkEntity;
    private bool _isInitialized = false;

    [GONetAutoMagicalSync("EntityStateSynchronizer_TargetEntityId")] public uint targetEntityId;
    [GONetAutoMagicalSync("EntityStateSynchronizer_MovementInput")] public Vector2 movementInput;
    [GONetAutoMagicalSync("EntityStateSynchronizer_CameraLookAEulerAngles")] public Vector3 cameraLookAtEulerAngles;
    [GONetAutoMagicalSync("EntityStateSynchronizer_VelocityVector")] public Vector3 velocityVector;

    [GONetAutoMagicalSync("_GONet_EntityState_NoInterpolated")] public bool isGrounded;
    [GONetAutoMagicalSync("_GONet_EntityState_NoInterpolated")] public bool isCrouched;

    private Subscription<SyncEvent_ValueChangeProcessed> _targetEntityIdSubscription;

    public bool HasTargetNetworkEntity => _isInitialized;

    protected override void Awake()
    {
        base.Awake();
        _targetEntityIdSubscription = GONetMain.EventBus.Subscribe(SyncEvent_GeneratedTypes.SyncEvent_EntityStateSynchronizer_targetEntityId, OnTargetEntityIdChanged);
    }

    public override void OnGONetParticipantStarted()
    {
        base.OnGONetParticipantStarted();

        if(IsClient)
        {
            _isInitialized = FindTargetEntityId();
        }
    }

    public override void OnGONetParticipantDisabled()
    {
        if(IsClient)
        {
            _targetEntityIdSubscription.Unsubscribe();
        }

        base.OnGONetParticipantDisabled();
    }

    private void OnTargetEntityIdChanged(GONetEventEnvelope<SyncEvent_ValueChangeProcessed> eventEnvelope)
    {
        if(eventEnvelope.GONetParticipant.GONetId != gonetParticipant.GONetId)
        {
            return;
        }

        _isInitialized = FindTargetEntityId();
    }

    private bool FindTargetEntityId()
    {
        GONetParticipant[] activeParticipants = FindObjectsOfType<GONetParticipant>(true);
        bool foundSuccesfully = false;

        for (int i = 0; i < activeParticipants.Length; i++)
        {
            if (activeParticipants[i].GONetId == targetEntityId)
            {
                if (activeParticipants[i].TryGetComponent<INetworkEntity>(out _targetNetworkEntity))
                {
                    foundSuccesfully = true;
                    break;
                }
            }
        }

        return foundSuccesfully;
    }

    /// <summary>
    /// This is only supported on server. Updates the all the GONetAutoMagicalSyncValues with the information of the most recent entity state within its target network entity.
    /// </summary>
    public void UpdateEntityStateServerOnly()
    {
        if (!_isInitialized || !IsServer) return;

        EntityState updatedEntityState = _targetNetworkEntity.GetEntityStateWithoutEvent();

        transform.position = updatedEntityState.position;

        movementInput = updatedEntityState.movementInput;

        cameraLookAtEulerAngles = updatedEntityState.cameraLookAtEulerAngles;

        velocityVector = updatedEntityState.velocityVector;

        isGrounded = updatedEntityState.isGrounded;
        isCrouched = updatedEntityState.isCrouched;
    }

    private void Update()
    {
        if (!_isInitialized || IsServer)
        {
            return;
        }

        //This condition has been added because if not Temporary Entities create errors when their GONetParticipants are null
        if (_targetNetworkEntity.GetNetworkObject() == null)
        {
            _isInitialized = false;
            return;
        }

        if(IsClient && _targetNetworkEntity.GetNetworkObject().IsMine)
        {
            return;
        }

        EntityState entityState = new EntityState(targetEntityId, movementInput, transform.position,
                                                  cameraLookAtEulerAngles, velocityVector, isGrounded, isCrouched);

        _targetNetworkEntity.ReceiveEntityState(entityState);
    }
}
