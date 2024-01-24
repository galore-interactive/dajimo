using GONet;
using System.Collections.Generic;

public class DroneLifetimeHandler
{
    private readonly float _secondsToSpawnAgain;
    private readonly NetworkController _networkController;
    private readonly Dictionary<uint, float> _activeDronesSecondsRemainingByDroneId;
    private readonly Dictionary<uint, float> _activeDronesModifications;
    private readonly HashSet<uint> _dronesToDelete;

    public DroneLifetimeHandler(NetworkController networkController, float secondsToSpawnAgain)
    {
        _secondsToSpawnAgain = secondsToSpawnAgain;
        _networkController = networkController;

        _activeDronesSecondsRemainingByDroneId = new Dictionary<uint, float>();
        _activeDronesModifications = new Dictionary<uint, float>();
        _dronesToDelete = new HashSet<uint>();
    }

    public void Server_Update(float deltaTime)
    {
        foreach (uint droneId in _activeDronesSecondsRemainingByDroneId.Keys)
        {
            _activeDronesModifications[droneId] = _activeDronesSecondsRemainingByDroneId[droneId] - deltaTime;
            //GONetLog.Debug("counting down to kill drone /// " + _activeDronesModifications[droneId]);
        }

        foreach (KeyValuePair<uint, float> kvp in _activeDronesModifications)
        {
            uint droneId = kvp.Key;
            float secondsRemaining = kvp.Value;
            _activeDronesSecondsRemainingByDroneId[droneId] = secondsRemaining;

            if (secondsRemaining <= 0)
            {
                //GONetLog.Debug("going to kill drone");
                _networkController.Server_InitiateKillPlayerProcess(droneId);
                _dronesToDelete.Add(droneId);
            }
        }

        foreach (uint droneId in _dronesToDelete)
        {
            _activeDronesSecondsRemainingByDroneId.Remove(droneId);
        }

        _activeDronesModifications.Clear();
        _dronesToDelete.Clear();
    }

    public void AddDroneIfNotPresent(uint entityId)
    {
        if (_activeDronesSecondsRemainingByDroneId.ContainsKey(entityId)) return;
        
        //GONetLog.Debug("adding to kill drone");
        _activeDronesSecondsRemainingByDroneId.Add(entityId, _secondsToSpawnAgain);
    }
}
