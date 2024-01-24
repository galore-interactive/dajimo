using UnityEngine;

public class GameTimer : MonoBehaviour
{
    private IGameTime _gameTimer;
    private bool _isInitialized = false;

    public void Init(IGameTime gameTimer)
    {
        _gameTimer = gameTimer;
        _isInitialized = true;
    }

    private void Update()
    {
        if(!_isInitialized)
        {
            return;
        }

        _gameTimer.Update();
    }

    public float GetCurrentTime()
    {
        if(!_isInitialized)
        {
            return 0f;
        }

        return _gameTimer.GetCurrentTime();
    }
}
