public interface IPlayerNetworkEntity : INetworkEntity
{
    public void Simulate(I_InputState inputState, float elapsedTime, float serverTime);
    public IPlayerState GetPlayerState();
    public void ReceivePlayerState(IPlayerState playerState);

    /// <summary>
    /// This method is used in client-side to activate the playable entity in order to be controlled by the player again. This is only supported for IsMine_ToRemotelyControl entities!
    /// </summary>
    public void ActivateEntityControl();

    /// <summary>
    /// This method is used in client-side to deactivate the playable entity in case we want to start controlling another IPlayerNetworkEntity entity. This is only supported for IsMine_ToRemotelyControl entities!
    /// </summary>
    public void DeactivateEntityControl();
    public bool IsActivated { get; }
    public bool IsSpawned { get; }
    public PlayableEntityType GetPlayableEntityType();
}
