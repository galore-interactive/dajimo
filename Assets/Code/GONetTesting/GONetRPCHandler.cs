using GONet;
using MemoryPack;
using System;
using UnityEngine;

[Obsolete("Going pure GONet.")]
[MemoryPackable]
public partial class MyEvent : ITransientEvent
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

    public ulong networkObjectID;
    public uint clientTick;

    public MyEvent(Vector3 position, Vector3 cameraLookAtEulerAngles, Vector2 movementInput, Vector3 movementVector, Vector3 velocityVector,
                       Vector3 smoothDampVelocityVector, float verticalSpeed, bool isGrounded, float headVerticalPosition, float characterControllerHeight,
                       Vector3 characterControllerCenter, Vector3 recoilRotationAmountLeft, int recoilPredictableRandomValueIndex, bool isMoving, bool isMovingBackwards,
                       bool isWalking, bool isRunning, bool isCrouched, bool isAiming, bool isShooting, bool isReloading, float timeSinceLastShot, ulong networkObjectID,
                       bool isPerformingCrouchTransition, float currentCrouchTransitionTime, float headOriginCrouchVerticalPosition, float headTargetCrouchVerticalPosition,
                       Vector3 crouchTargetCharacterControllerCenter, Vector3 crouchOriginCharacterControllerCenter, float crouchTargetCharacterControllerHeight,
                       float crouchOriginCharacterControllerHeight, int activeWeaponAmmoLeft, int activeWeaponClipAmmoLeft, float activeWeaponReloadTimeLeft, uint clientTick)
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
    }

    [MemoryPackIgnore]
    public long OccurredAtElapsedTicks => 20;
}

[MemoryPackable] //TODO: Remove this as soon as GONet support it! 
public partial class InputStateEvent : ITransientEvent
{
    public string message;

    public long OccurredAtElapsedTicks => 0;

    public InputStateEvent(string message)
    {
        this.message = message;
    }
}

[MemoryPackable]//TODO: Remove this as soon as GONet support it! 
public partial class SnapshotEvent : ITransientEvent
{
    public string message;

    public long OccurredAtElapsedTicks => 0;

    public SnapshotEvent(string message)
    {
        this.message = message;
    }
}

public class GONetRPCHandler
{
}
