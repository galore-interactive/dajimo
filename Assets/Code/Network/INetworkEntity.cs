using GONet;
public interface INetworkEntity : INetworkEntityID
{
    public void ReceiveEntityState(EntityState entityState);
    public void ReceiveEntityEvent(LocalContextEntityEvent entityEvent);
    public EntityState GetEntityStateWithoutEvent();
    public ushort GetClientOwnerId();
    public bool IsPlayer();
    public GONetParticipant GetNetworkObject();
    public bool DoReceiveEntityStates();
    public void Server_SetCollidersVisibility(bool visibility);
}
