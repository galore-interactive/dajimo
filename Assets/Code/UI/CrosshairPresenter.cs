using UnityEngine;

public class CrosshairPresenter : MonoBehaviour
{
    private Crosshair _crosshair;
    private PlayerController _playerController;
    private bool _isLinked = false;

    private void Awake()
    {
        _crosshair = FindObjectOfType<Crosshair>(true);
    }

    public void SetPlayerController(PlayerController playerController)
    {
        _playerController = playerController;
        _isLinked = true;
    }

    public void RemovePlayerController()
    {
        _playerController = null;
        _isLinked = false;
    }

    public void EnableCrosshairUpdate()
    {
        if(_isLinked)
        {
            _playerController.OnPlayerStateChanged += UpdateCrosshair;
        }
    }

    private void UpdateCrosshair(BipedPlayerState playerState)
    {
        bool visibility = CalculateCrosshairVisibilityStatus(playerState.isAiming);
        float size = CalculateCrosshairSize(playerState.isCrouched, playerState.isMoving, playerState.isRunning, playerState.isAiming);

        _crosshair.SetSize(size);
        _crosshair.SetCenterDotVisibility(playerState.isAiming); // only show the center dot if aiming, otherwise what is the real purpose of aiming?
        if(visibility)
        {
            _crosshair.Show();
        }
        else
        {
            _crosshair.Hide();
        }
    }

    private bool CalculateCrosshairVisibilityStatus(bool isAiming)
    {
        if(isAiming)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private float CalculateCrosshairSize(bool isCrouched, bool isMoving, bool isRunning, bool isAiming)
    {
        if (isAiming)
        {
            return 11;
        }
        else if(isCrouched)
        {
            return 25f;
        }
        else if(isRunning)
        {
            return 150f;
        }
        else if(isMoving)
        {
            return 100f;
        }
        else
        {
            return 50;
        }
    }

    public void DisableCrosshairUpdate()
    {
        if (_isLinked)
        {
            _playerController.OnPlayerStateChanged -= UpdateCrosshair;
        }
    }
}
