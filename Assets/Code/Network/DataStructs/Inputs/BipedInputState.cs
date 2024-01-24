using UnityEngine;
using MemoryPack;

[MemoryPackable]
public partial class BipedInputState : I_InputState
{
    public Vector2 movementInput;
    public Vector2 cameraMovementInput;
    public bool isRunning;
    public bool isCrouchingOrStandingUp;
    public bool isAiming;
    public bool isShooting;
    public bool isReloading;
    public bool isSmashing;
    public uint clientTick;
    public PlayableEntityType entityType;

    public BipedInputState(Vector2 movementInput, Vector2 cameraMovementInput, bool isRunning, bool isCrouchingOrStandingUp, bool isAiming, bool isShooting, bool isReloading, bool isSmashing, uint clientTick)
    {
        this.movementInput = movementInput;
        this.cameraMovementInput = cameraMovementInput;
        this.isRunning = isRunning;
        this.isCrouchingOrStandingUp = isCrouchingOrStandingUp;
        this.isAiming = isAiming;
        this.isShooting = isShooting;
        this.isReloading = isReloading;
        this.isSmashing = isSmashing;
        this.clientTick = clientTick;

        this.entityType = PlayableEntityType.Biped;
    }

    [MemoryPackIgnore]
    public PlayableEntityType EntityType => entityType;
    [MemoryPackIgnore]
    public uint ClientTick => clientTick;

    [MemoryPackIgnore]
    public long OccurredAtElapsedTicks => 0;
    public void SetClientTick(uint value)
    {
        clientTick = value;
    }

    public override string ToString()
    {
        return JsonUtility.ToJson(this);
    }
}
