using GONet;

public class ObstacleController : GONetParticipantCompanionBehaviour, IDamageable, INetworkEntity
{
    private Health _healthComponent;
    private NetworkController _networkController;

    public override void OnGONetParticipantStarted()
    {
        base.OnGONetParticipantStarted();

        if (IsServer)
        {
            _networkController = FindObjectOfType<NetworkController>();
            _healthComponent = new Health(10);

            _healthComponent.OnKilled += Server_OnKilled;
        }
    }

    private void Server_OnKilled()
    {
        _networkController.Server_KillEntity(GetNetworkEntityId());
    }

    public bool DoReceiveEntityStates()
    {
        return false;
    }

    public ushort GetClientOwnerId()
    {
        return gonetParticipant.OwnerAuthorityId;
    }

    public EntityState GetEntityStateWithoutEvent()
    {
        throw new System.NotImplementedException();
    }

    public uint GetNetworkEntityId()
    {
        return gonetParticipant.GONetId;
    }

    public GONetParticipant GetNetworkObject()
    {
        return gonetParticipant;
    }

    public bool IsPlayer()
    {
        return false;
    }

    public bool IsRewindable()
    {
        return false;
    }

    public void ReceiveEntityEvent(LocalContextEntityEvent entityEvent)
    {
        throw new System.NotImplementedException();
    }

    public void ReceiveEntityState(EntityState entityState)
    {
        throw new System.NotImplementedException();
    }

    public void Rewind(float rewindedTime, bool interpolate = false)
    {
        throw new System.NotImplementedException();
    }

    public void Server_SetCollidersVisibility(bool visibility)
    {
        throw new System.NotImplementedException();
    }

    public void Simulate(float elapsedTime, float serverTime)
    {

    }

    public void Server_TakeDamage(int damage)
    {
        if (IsServer)
        {
            _healthComponent.TakeDamage(damage);
        }
    }
}
