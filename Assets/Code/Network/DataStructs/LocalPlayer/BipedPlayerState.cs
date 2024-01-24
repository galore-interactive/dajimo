using UnityEngine;
using MemoryPack;
using System;

[MemoryPackable]
public partial class BipedPlayerState : IPlayerState
{
    //Position
    public Vector3 position;

    //Movement
    public Vector2 movementInput;
    public Vector3 smoothDampVelocityVector;
    public Vector3 movementVector;
    public Vector3 velocityVector; //For entities and extrapolation
    public bool isGrounded; //For entities and extrapolation
    public float verticalSpeed;
    public bool isMoving;
    public bool isMovingBackwards;
    public bool isWalking;
    public bool isRunning;

    //Rotation
    public Vector3 cameraLookAtEulerAngles;

    //Crouch-Stand
    public float headVerticalPosition;
    public float headOriginCrouchVerticalPosition;
    public float headTargetCrouchVerticalPosition;
    public float characterControllerHeight;
    public Vector3 characterControllerCenter;
    public bool isCrouched;
    public bool isPerformingCrouchTransition;
    public float currentCrouchTransitionTime;
    public Vector3 crouchTargetCharacterControllerCenter;
    public Vector3 crouchOriginCharacterControllerCenter;
    public float crouchTargetCharacterControllerHeight;
    public float crouchOriginCharacterControllerHeight;

    //Recoil
    public Vector3 recoilRotationAmountLeft;
    public int recoilPredictableRandomValueIndex;

    //Aiming
    public bool isAiming;

    //Shooting
    public float timeSinceLastShot;
    public bool isShooting;

    //Active weapon
    public int activeWeaponClipAmmoLeft;
    public int activeWeaponAmmoLeft;

    //Reloading
    public bool isReloading;
    public float activeWeaponReloadTimeLeft;

    //Smash
    public bool isSmashing;
    public float smashTimeLeft;

    public ulong networkObjectID;
    public uint clientTick;
    PlayableEntityType playableEntityType;

    public BipedPlayerState() { }

    [MemoryPackConstructor]
    public BipedPlayerState(Vector3 position, Vector2 movementInput, Vector3 smoothDampVelocityVector, Vector3 movementVector, Vector3 velocityVector,
                       bool isGrounded, float verticalSpeed, bool isMoving, bool isMovingBackwards, bool isWalking, bool isRunning,
                       Vector3 cameraLookAtEulerAngles, float headVerticalPosition, float headOriginCrouchVerticalPosition,
                       float headTargetCrouchVerticalPosition, float characterControllerHeight, Vector3 characterControllerCenter, bool isCrouched,
                       bool isPerformingCrouchTransition, float currentCrouchTransitionTime, Vector3 crouchTargetCharacterControllerCenter,
                       Vector3 crouchOriginCharacterControllerCenter, float crouchTargetCharacterControllerHeight, float crouchOriginCharacterControllerHeight,
                       Vector3 recoilRotationAmountLeft, int recoilPredictableRandomValueIndex, bool isAiming, float timeSinceLastShot, bool isShooting,
                       int activeWeaponClipAmmoLeft, int activeWeaponAmmoLeft, bool isReloading, float activeWeaponReloadTimeLeft, bool isSmashing, float smashTimeLeft, ulong networkObjectID, uint clientTick)
    {
        this.position = position;
        this.movementInput = movementInput;
        this.movementVector = movementVector;
        this.velocityVector = velocityVector;
        this.smoothDampVelocityVector = smoothDampVelocityVector;
        this.verticalSpeed = verticalSpeed;
        this.isGrounded = isGrounded;
        this.headVerticalPosition = headVerticalPosition;
        this.characterControllerHeight = characterControllerHeight;
        this.characterControllerCenter = characterControllerCenter;
        this.recoilRotationAmountLeft = recoilRotationAmountLeft;
        this.recoilPredictableRandomValueIndex = recoilPredictableRandomValueIndex;
        this.cameraLookAtEulerAngles = cameraLookAtEulerAngles;
        this.isMoving = isMoving;
        this.isMovingBackwards = isMovingBackwards;
        this.isWalking = isWalking;
        this.isRunning = isRunning;
        this.isCrouched = isCrouched;
        this.isAiming = isAiming;
        this.isShooting = isShooting;
        this.isReloading = isReloading;
        this.isSmashing = isSmashing;
        this.smashTimeLeft = smashTimeLeft;
        this.timeSinceLastShot = timeSinceLastShot;
        this.networkObjectID = networkObjectID;
        this.isPerformingCrouchTransition = isPerformingCrouchTransition;
        this.currentCrouchTransitionTime = currentCrouchTransitionTime;
        this.headOriginCrouchVerticalPosition = headOriginCrouchVerticalPosition;
        this.headTargetCrouchVerticalPosition = headTargetCrouchVerticalPosition;
        this.crouchTargetCharacterControllerCenter = crouchTargetCharacterControllerCenter;
        this.crouchOriginCharacterControllerCenter = crouchOriginCharacterControllerCenter;
        this.crouchTargetCharacterControllerHeight = crouchTargetCharacterControllerHeight;
        this.crouchOriginCharacterControllerHeight = crouchOriginCharacterControllerHeight;
        this.activeWeaponAmmoLeft = activeWeaponAmmoLeft;
        this.activeWeaponClipAmmoLeft = activeWeaponClipAmmoLeft;
        this.activeWeaponReloadTimeLeft = activeWeaponReloadTimeLeft;
        this.clientTick = clientTick;

        playableEntityType = PlayableEntityType.Biped;
    }

    [MemoryPackIgnore]
    public PlayableEntityType PlayableEntityType => playableEntityType;
    [MemoryPackIgnore]
    public long OccurredAtElapsedTicks => 0;
    [MemoryPackIgnore]
    public uint ClientTick => clientTick;
    public void GetShotPointAndShotDirection(out Vector3 shotPoint, out Vector3 shotLookAtDirection)
    {
        shotPoint = new Vector3(position.x, headVerticalPosition, position.z);
        shotLookAtDirection = cameraLookAtEulerAngles;
    }

    public bool AreNearlyEqual(IPlayerState otherPlayerState, float tolerance)
    {
        if(otherPlayerState.PlayableEntityType != PlayableEntityType)
        {
            return false;
        }

        BipedPlayerState otherBipedPlayerState = (BipedPlayerState)otherPlayerState;

        Vector3 playerPositionDifference = VectorUtils.GetDifferenceBetweenTwoVector3(position, otherBipedPlayerState.position);
        Vector3 playercamereraLookAtEulerAnglesDifference = VectorUtils.GetDifferenceBetweenTwoVector3(cameraLookAtEulerAngles, otherBipedPlayerState.cameraLookAtEulerAngles);
        Vector3 playerCharacterControllerCenterPositionDifference = VectorUtils.GetDifferenceBetweenTwoVector3(characterControllerCenter, otherBipedPlayerState.characterControllerCenter);
        Vector3 playerWeaponRecoilAmountLeftDifference = VectorUtils.GetDifferenceBetweenTwoVector3(recoilRotationAmountLeft, otherBipedPlayerState.recoilRotationAmountLeft);
        Vector3 crouchTargetCharacterControllerCenterDifference = VectorUtils.GetDifferenceBetweenTwoVector3(crouchTargetCharacterControllerCenter, otherBipedPlayerState.crouchTargetCharacterControllerCenter);
        Vector3 crouchOriginCharacterControllerCenterDifference = VectorUtils.GetDifferenceBetweenTwoVector3(crouchOriginCharacterControllerCenter, otherBipedPlayerState.crouchOriginCharacterControllerCenter);

        if (isMoving != otherBipedPlayerState.isMoving ||
            isRunning != otherBipedPlayerState.isRunning ||
            isWalking != otherBipedPlayerState.isWalking ||
            isAiming != otherBipedPlayerState.isAiming ||
            isShooting != otherBipedPlayerState.isShooting ||
            isCrouched != otherBipedPlayerState.isCrouched ||
            isReloading != otherBipedPlayerState.isReloading ||
            isSmashing != otherBipedPlayerState.isSmashing ||
            isPerformingCrouchTransition != otherBipedPlayerState.isPerformingCrouchTransition ||
            recoilPredictableRandomValueIndex != otherBipedPlayerState.recoilPredictableRandomValueIndex ||
            (Mathf.Abs(currentCrouchTransitionTime - otherBipedPlayerState.currentCrouchTransitionTime)) > tolerance ||
            (Mathf.Abs(headOriginCrouchVerticalPosition - otherBipedPlayerState.headOriginCrouchVerticalPosition)) > tolerance ||
            (Mathf.Abs(headTargetCrouchVerticalPosition - otherBipedPlayerState.headTargetCrouchVerticalPosition)) > tolerance ||
            (Mathf.Abs(timeSinceLastShot - otherBipedPlayerState.timeSinceLastShot)) > tolerance ||
            (Mathf.Abs(smashTimeLeft - otherBipedPlayerState.smashTimeLeft)) > tolerance ||
            (Math.Abs(activeWeaponClipAmmoLeft - otherBipedPlayerState.activeWeaponClipAmmoLeft)) > 0 ||
            (Math.Abs(activeWeaponAmmoLeft - otherBipedPlayerState.activeWeaponAmmoLeft)) > 0 ||
            (Mathf.Abs(activeWeaponReloadTimeLeft - otherBipedPlayerState.activeWeaponReloadTimeLeft)) > tolerance ||
            VectorUtils.IsDifferenceGreaterThanTolerance(crouchTargetCharacterControllerCenterDifference, tolerance) ||
            VectorUtils.IsDifferenceGreaterThanTolerance(crouchOriginCharacterControllerCenterDifference, tolerance) ||
            (Mathf.Abs(crouchTargetCharacterControllerHeight - otherBipedPlayerState.crouchTargetCharacterControllerHeight)) > tolerance ||
            (Mathf.Abs(crouchOriginCharacterControllerHeight - otherBipedPlayerState.crouchOriginCharacterControllerHeight)) > tolerance ||
            VectorUtils.IsDifferenceGreaterThanTolerance(playerPositionDifference, tolerance) ||
            VectorUtils.IsDifferenceGreaterThanTolerance(playercamereraLookAtEulerAnglesDifference, tolerance) ||
            VectorUtils.IsDifferenceGreaterThanTolerance(playerCharacterControllerCenterPositionDifference, tolerance) ||
            VectorUtils.IsDifferenceGreaterThanTolerance(playerWeaponRecoilAmountLeftDifference, tolerance) ||
            (Mathf.Abs(characterControllerHeight - otherBipedPlayerState.characterControllerHeight)) > tolerance ||
            (Mathf.Abs(headVerticalPosition - otherBipedPlayerState.headVerticalPosition)) > tolerance)
        {
            Debug.Log($"Client Pos: {otherBipedPlayerState.position} || Server Pos: {position} || Difference: {playerPositionDifference} " +
                $"|| Tick: {ClientTick}, {otherBipedPlayerState.clientTick} || Position sync failing: {VectorUtils.IsDifferenceGreaterThanTolerance(playerPositionDifference, tolerance)}\n" +
                $"Client LookAt: {otherBipedPlayerState.cameraLookAtEulerAngles} || Server LookAt: {cameraLookAtEulerAngles} " +
                $"|| LookAt difference: {playercamereraLookAtEulerAnglesDifference} || LookAt sync failing: {VectorUtils.IsDifferenceGreaterThanTolerance(playercamereraLookAtEulerAnglesDifference, tolerance)}");
            return false;
        }
        else
        {
            return true;
        }
    }
}
