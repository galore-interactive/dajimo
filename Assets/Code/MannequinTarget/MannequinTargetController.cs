using System.Collections.Generic;
using GONet;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(HealthController))]
public class MannequinTargetController : GONetParticipantCompanionBehaviour, INetworkEntity
{
    private HealthController _healthController;
    private Transform _transform;

    [SerializeField] private Collider _headCollider;
    [SerializeField] private Collider _bodyCollider;

    private MannequinMovement _movement;
    public Transform _waypoint1;
    public Transform _waypoint2;
    private float _movementSpeed = 3f;

    private NetworkController _networkController;

    private EntityState _mostRecentEntityState;

    private bool _isSpawned = false;

    private struct ServerMannequinState
    {
        public float time;
        public Vector3 position;

        public ServerMannequinState(Vector3 position, float time)
        {
            this.position = position;
            this.time = time;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        _networkController = FindObjectOfType<NetworkController>();
        _healthController = GetComponent<HealthController>();
        _healthController.Init(1000000);

        _transform = transform;
    }

    //OnNetworkSpawn
    public override void OnGONetParticipantStarted()
    {
        base.OnGONetParticipantStarted();
        if(IsServer)
        {
            _movement = new MannequinMovement(_waypoint1, _waypoint2, _movementSpeed);

            _healthController.OnKill += Kill;
        }

        _isSpawned = true;
    }

    //OnNetworkDespawn
    public override void OnGONetParticipantDisabled()
    {
        if(IsServer)
        {
            _healthController.OnKill -= Kill;
        }
        base.OnGONetParticipantDisabled();
    }

    private void Update()
    {
        if(!_isSpawned)
        {
            return;
        }

        if(IsClient)
        {
            _transform.position = _mostRecentEntityState.position;
            return;
        }

        return;
    }

    private void Kill()
    {
        _networkController.Server_KillEntity(GetNetworkEntityId());
    }

    public void ReceiveEntityState(EntityState entityState)
    {
        _mostRecentEntityState = entityState;
    }

    public void ReceiveEntityEvent(LocalContextEntityEvent entityEvent)
    {
        Debug.Log("Mando entityevent a mannequin");
    }

    public EntityState GetEntityStateWithoutEvent()
    {
        return new EntityState(GetNetworkEntityId(), Vector2.zero, _transform.position, _transform.eulerAngles, Vector3.zero, false, false);
    }

    public ushort GetClientOwnerId()
    {
        return GONetParticipant.OwnerAuthorityId;
    }

    public bool IsPlayer()
    {
        return false;
    }

    public void Simulate(float elapsedTime, float serverTime)
    {
        _movement.SimulateMovement(elapsedTime);
        _transform.position = _movement.CurrentPosition;
    }

    public bool IsRewindable()
    {
        return true;
    }

    public GONetParticipant GetNetworkObject()
    {
        return GONetParticipant;
    }

    public uint GetNetworkEntityId()
    {
        return GONetParticipant.GONetId;
    }

    public bool DoReceiveEntityStates()
    {
        return true;
    }

    public void Server_SetCollidersVisibility(bool visibility)
    {
        throw new System.NotImplementedException();
    }
}
