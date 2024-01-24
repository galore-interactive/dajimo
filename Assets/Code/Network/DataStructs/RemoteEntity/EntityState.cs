using UnityEngine;

public struct EntityState
{
    public ulong networkObjectID;
    public Vector2 movementInput;
    public Vector3 position;
    public Vector3 cameraLookAtEulerAngles;
    public Vector3 velocityVector;
    public bool isGrounded;
    public bool isCrouched;

    public EntityState(ulong networkObjectID, Vector2 movementInput, Vector3 position, Vector3 cameraLookAtEulerAngles, Vector3 velocityVector, bool isGrounded, bool isCrouched)
    {
        this.networkObjectID = networkObjectID;
        this.movementInput = movementInput;
        this.position = position;
        this.cameraLookAtEulerAngles = cameraLookAtEulerAngles;
        this.velocityVector = velocityVector;
        this.isGrounded = isGrounded;
        this.isCrouched = isCrouched;
    }
}
