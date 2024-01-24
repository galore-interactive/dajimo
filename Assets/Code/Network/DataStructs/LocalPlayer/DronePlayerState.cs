using UnityEngine;
using MemoryPack;

[MemoryPackable]
public partial class DronePlayerState : IPlayerState
{
    //Position
    public Vector3 position;

    //Movement
    public Vector2 movementInput;
    public Vector3 smoothDampVelocityVector;
    public Vector3 movementVector;
    public bool isMoving;

    //Rotation
    public Vector3 cameraLookAtEulerAngles;

    //Object placement
    public bool isPlacingObject;
    public float timeUntilObjectIsPlaced;

    public ulong networkObjectID;
    public uint clientTick;
    PlayableEntityType playableEntityType;

    [MemoryPackIgnore]
    public PlayableEntityType PlayableEntityType => playableEntityType;
    [MemoryPackIgnore]
    public uint ClientTick => clientTick;
    [MemoryPackIgnore]
    public long OccurredAtElapsedTicks => throw new System.NotImplementedException();

    public DronePlayerState(Vector3 position, Vector2 movementInput, Vector3 smoothDampVelocityVector, Vector3 movementVector, bool isMoving,
                            Vector3 cameraLookAtEulerAngles, bool isPlacingObject, float timeUntilObjectIsPlaced, ulong networkObjectID, uint clientTick)
    {
        this.position = position;
        this.movementInput = movementInput;
        this.smoothDampVelocityVector = smoothDampVelocityVector;
        this.movementVector = movementVector;
        this.isMoving = isMoving;
        this.cameraLookAtEulerAngles = cameraLookAtEulerAngles;
        this.isPlacingObject = isPlacingObject;
        this.timeUntilObjectIsPlaced = timeUntilObjectIsPlaced;
        this.networkObjectID = networkObjectID;
        this.clientTick = clientTick;

        playableEntityType = PlayableEntityType.Drone;
    }

    public bool AreNearlyEqual(IPlayerState otherPlayerState, float tolerance)
    {
        if(otherPlayerState.PlayableEntityType != PlayableEntityType)
        {
            return false;
        }

        DronePlayerState otherDronePlayerState = (DronePlayerState)otherPlayerState;

        Vector3 playerPositionDifference = VectorUtils.GetDifferenceBetweenTwoVector3(position, otherDronePlayerState.position);
        Vector3 playercamereraLookAtEulerAnglesDifference = VectorUtils.GetDifferenceBetweenTwoVector3(cameraLookAtEulerAngles, otherDronePlayerState.cameraLookAtEulerAngles);

        if (isMoving != otherDronePlayerState.isMoving ||
            VectorUtils.IsDifferenceGreaterThanTolerance(playerPositionDifference, tolerance) ||
            VectorUtils.IsDifferenceGreaterThanTolerance(playercamereraLookAtEulerAnglesDifference, tolerance) ||
            isPlacingObject != otherDronePlayerState.isPlacingObject ||
            (Mathf.Abs(timeUntilObjectIsPlaced - otherDronePlayerState.timeUntilObjectIsPlaced)) > tolerance)
        {
            Debug.Log($"Client Pos: {otherDronePlayerState.position} || Server Pos: {position} || Difference: {playerPositionDifference} " +
                $"|| Tick: {ClientTick}, {otherDronePlayerState.clientTick} || Position sync failing: {VectorUtils.IsDifferenceGreaterThanTolerance(playerPositionDifference, tolerance)}\n" +
                $"Client LookAt: {otherDronePlayerState.cameraLookAtEulerAngles} || Server LookAt: {cameraLookAtEulerAngles} " +
                $"|| LookAt difference: {playercamereraLookAtEulerAnglesDifference} || LookAt sync failing: {VectorUtils.IsDifferenceGreaterThanTolerance(playercamereraLookAtEulerAnglesDifference, tolerance)}");
            return false;
        }
        else
        {
            return true;
        }
    }

    public void GetShotPointAndShotDirection(out Vector3 shotPoint, out Vector3 shotLookAtDirection)
    {
        throw new System.NotImplementedException();
    }
}
