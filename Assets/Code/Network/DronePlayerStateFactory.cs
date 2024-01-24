using GONet;
using UnityEngine;

public class DronePlayerStateFactory : IPlayerStateFactory
{
    private readonly PlayerClientNoInterpolationGetter _noInterpolatedComponents;
    private readonly DroneMovement _droneMovement;
    private readonly PlayerStateVariables _stateVariables;
    private readonly GONetParticipant _gonetParticipant;
    private readonly ObstaclesCreator _obstaclesCreator;

    public DronePlayerStateFactory(PlayerClientNoInterpolationGetter noInterpolatedComponents, DroneMovement droneMovement, PlayerStateVariables stateVariables, GONetParticipant gonetParticipant, ObstaclesCreator obstaclesCreator)
    {
        _noInterpolatedComponents = noInterpolatedComponents;
        _droneMovement = droneMovement;
        _stateVariables = stateVariables;
        _gonetParticipant = gonetParticipant;
        _obstaclesCreator = obstaclesCreator;
    }

    public IPlayerState Create()
    {
        return CreateSpecific();
    }

    public DronePlayerState CreateSpecific()
    {
        Vector3 cameraLookAtEulerAngles = _noInterpolatedComponents.NoInterpolatedSplitRotationSource.rotation.eulerAngles;

        return new DronePlayerState(_noInterpolatedComponents.NoInterpolatedPlayerPositionSource.position,
                                    _droneMovement.MovementInput,
                                    _droneMovement.SmoothDampVelocity,
                                    _droneMovement.CurrentMovementVector,
                                    _stateVariables.IsMoving,
                                    cameraLookAtEulerAngles,
                                    _obstaclesCreator.IsPlacingObject,
                                    _obstaclesCreator.TimeUntilObjectIsPlaced,
                                    _gonetParticipant.GONetId,
                                    _stateVariables.ClientTick);
    }
}
