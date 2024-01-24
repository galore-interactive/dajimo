public class PlayerStateVariables
{
    private bool _isMoving = false;
    private bool _isMovingBackwards = false;
    private bool _isWalking = false;
    private bool _isRunning = false;

    private bool _isCrouched = false;

    private bool _isReloading = false;
    private bool _isAiming = false;
    private bool _isShooting = false;
    private bool _isHidingWeapon = false;
    private bool _isSmashing;
    private uint _clientTick;

    public bool IsReloading => _isReloading;
    public bool IsAiming => _isAiming;
    public bool IsRunning => _isRunning;
    public bool IsShooting => _isShooting;
    public bool IsMoving => _isMoving;
    public bool IsMovingBackwards => _isMovingBackwards;
    public bool IsWalking => _isWalking;
    public bool IsHidingWeapon => _isHidingWeapon;
    public bool IsCrouched => _isCrouched;
    public bool IsSmashing => _isSmashing;
    public uint ClientTick => _isServer ? _clientTick : _networkController.ClientTick;

    private readonly NetworkController _networkController;
    private readonly bool _isServer;

    public void SetIsReloading(bool newState)
    {
        _isReloading = newState;
    }

    public void SetClientTick(uint newValue)
    {
        if(_isServer)
        {
            _clientTick = newValue;
        }
        else
        {
            _networkController.SetClientTick(newValue);
        }
    }

    public void SetIsAiming(bool newState)
    {
        _isAiming = newState;
    }

    public void SetIsRunning(bool newState)
    {
        _isRunning = newState;
    }

    public void SetIsShooting(bool newState)
    {
        _isShooting = newState;
    }

    public void SetIsMoving(bool newState)
    {
        _isMoving = newState;
    }

    public void SetIsMovingBackwards(bool newState)
    {
        _isMovingBackwards = newState;
    }

    public void SetIsWalking(bool newState)
    {
        _isWalking = newState;
    }

    public void SetIsHidingWeapon(bool newState)
    {
        _isHidingWeapon = newState;
    }

    public void SetIsCrouched(bool newState)
    {
        _isCrouched = newState;
    }

    public void SetIsSmashing(bool newState)
    {
        _isSmashing = newState;
    }

    public PlayerStateVariables(NetworkController networkController, bool isServer)
    {
        _networkController = networkController;
        _isServer = isServer;

    }
}
