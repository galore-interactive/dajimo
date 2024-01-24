public class DroneAndBipedSwitcher
{
    //Server side variables
    private IPlayerNetworkEntity _playerNetworkEntityToReplace;
    private readonly DroneLifetimeHandler _droneLifetimeHandler;
    private ServerSimulator _serverSimulator;

    //Client side variables
    private readonly NetworkController _networkController;

    //Client-side constructor
    public DroneAndBipedSwitcher(NetworkController networkController)
    {
        _networkController = networkController;
    }
    
    //Server-side constructor
    public DroneAndBipedSwitcher(ServerSimulator serverSimulator, DroneLifetimeHandler droneLifetimeHandler)
    {
        _serverSimulator = serverSimulator;
        _droneLifetimeHandler = droneLifetimeHandler;
    }

    /// <summary>
    /// This method is server side only. This method assigns the corresponding biped entity to the new drone entity IF POSSIBLE. 
    /// If it is not possible (aka no corresponding biped entity found), it will not assign anything.
    /// </summary>
    public void Server_StartTrackingDroneIfPossible(IPlayerNetworkEntity dronePlayableNetworkEntity)
    {
        _droneLifetimeHandler.AddDroneIfNotPresent(dronePlayableNetworkEntity.GetNetworkEntityId());
    }
}
