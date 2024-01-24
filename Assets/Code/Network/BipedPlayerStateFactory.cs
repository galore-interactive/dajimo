using UnityEngine;
using GONet;

public class BipedPlayerStateFactory : IPlayerStateFactory
{
    private readonly PlayerClientNoInterpolationGetter _noInterpolatedComponents;
    private readonly PlayerMovement _playerMovement;
    private readonly PlayerWeaponRecoilController _weaponRecoilController;
    private readonly PlayerStateVariables _stateVariables;
    private readonly WeaponsHandler _weaponsHandler;
    private readonly GONetParticipant _gonetParticipant;
    private readonly CrouchStandController _crouchStandComponent;
    private readonly SmashController _smashController;

    public BipedPlayerStateFactory(PlayerClientNoInterpolationGetter noInterpolatedComponents, PlayerMovement playerMovement, PlayerWeaponRecoilController weaponRecoilController, PlayerStateVariables stateVariables, WeaponsHandler weaponsHandler, CrouchStandController crouchStandComponent, SmashController smashController, GONetParticipant gonetParticipant)
    {
        _noInterpolatedComponents = noInterpolatedComponents;
        _playerMovement = playerMovement;
        _weaponRecoilController = weaponRecoilController;
        _stateVariables = stateVariables;
        _weaponsHandler = weaponsHandler;
        _crouchStandComponent = crouchStandComponent;
        _smashController = smashController;
        _gonetParticipant = gonetParticipant;
    }

    public IPlayerState Create()
    {
        return CreateSpecific();
    }

    public BipedPlayerState CreateSpecific()
    {
        Vector3 cameraLookAtEulerAngles = _noInterpolatedComponents.NoInterpolatedSplitRotationSource.rotation.eulerAngles;

        return new BipedPlayerState(
            _noInterpolatedComponents.NoInterpolatedPlayerPosition,
            _playerMovement.MovementInput,
            _playerMovement.SmoothDampVelocity,
            _playerMovement.CurrentMovementVector,
            _noInterpolatedComponents.CharacterController.velocity,
            _noInterpolatedComponents.CharacterController.isGrounded,
            _playerMovement.VerticalSpeed,
            _stateVariables.IsMoving,
            _stateVariables.IsMovingBackwards,
            _stateVariables.IsWalking,
            _stateVariables.IsRunning,
            cameraLookAtEulerAngles,
            _noInterpolatedComponents.NoInterpolatedHeadTransform.position.y,
            _crouchStandComponent.OriginVerticalPosition,
            _crouchStandComponent.TargetVerticalPosition,
            _noInterpolatedComponents.CharacterController.height,
            _noInterpolatedComponents.CharacterController.center,
            _stateVariables.IsCrouched,
            _crouchStandComponent.IsPerformingTransition,
            _crouchStandComponent.CurrentTransitionTime,
            _crouchStandComponent.TargetCharacterControllerCenter,
            _crouchStandComponent.OriginCharacterControllerCenter,
            _crouchStandComponent.TargetCharacterControllerHeight,
            _crouchStandComponent.OriginCharacterControllerHeight,
            _weaponRecoilController.RotationAmountLeft,
            _weaponRecoilController.PredictableRandomValueIndex,
            _stateVariables.IsAiming,
            _weaponsHandler.ActiveWeaponTimeSinceLastShot,
            _stateVariables.IsShooting,
            _weaponsHandler.ActiveWeaponClipAmmoLeft,
            _weaponsHandler.ActiveWeaponAmmoLeft,
            _stateVariables.IsReloading,
            _weaponsHandler.ActiveWeaponReloadTimeLeft,
            _stateVariables.IsSmashing,
            _smashController.SmashingTimeLeft,
            _gonetParticipant.GONetId,
            _stateVariables.ClientTick);
    }
}
