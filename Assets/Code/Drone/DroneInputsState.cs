using MemoryPack;
using UnityEngine;

[MemoryPackable]
public partial class DroneInputsState : I_InputState
{
    public Vector3 movementInput;
    public Vector2 mouseMovementInput;
    public bool isPlacingObject;
    [MemoryPackIgnore] public float placingObjectForwardY;
    public uint clientTick;
    public PlayableEntityType entityType;

    public DroneInputsState(Vector3 movementInput, Vector2 mouseMovementInput, bool isPlacingObject, uint clientTick)
    {
        this.movementInput = movementInput;
        this.mouseMovementInput = mouseMovementInput;
        this.isPlacingObject = isPlacingObject;
        this.clientTick = clientTick;

        entityType = PlayableEntityType.Drone;
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
}
