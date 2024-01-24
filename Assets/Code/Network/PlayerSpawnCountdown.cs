public class PlayerSpawnCountdown
{
    private readonly float _timeToSpawnAgain;
    private float _timeLeft = 3f;
    private NetworkController _networkController;
    private bool _isCountdownEnabled = true;
    private PlayableEntityType _nextEntityTypeToSpawn;

    public PlayerSpawnCountdown(NetworkController networkController, float timeToSpawnAgain)
    {
        _timeToSpawnAgain = timeToSpawnAgain;
        _networkController = networkController;

        _nextEntityTypeToSpawn = PlayableEntityType.None;
    }

    public void Update(float elapsedTime)
    {
        if(!_isCountdownEnabled)
        {
            return;
        }

        _timeLeft -= elapsedTime;
        if (_timeLeft <= 0f)
        {
            DisableCountdown();
            _timeLeft = _timeToSpawnAgain;
            _networkController.Client_SwitchToControlPlayerType(_nextEntityTypeToSpawn);
        }
    }

    public void EnableCountdown(PlayableEntityType nextPlayableEntityToSpawn)
    {
        _nextEntityTypeToSpawn = nextPlayableEntityToSpawn;
        _isCountdownEnabled = true;
    }

    public void DisableCountdown()
    {
        _isCountdownEnabled = false;
    }
}
