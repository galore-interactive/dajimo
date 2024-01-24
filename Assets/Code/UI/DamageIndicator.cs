using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator: MonoBehaviour
{
    private float _currentLifetime;
    private Vector3 _direction;
    private bool _isInitialized = false;

    public float CurtrentLifeTime => _currentLifetime;

    public void Initialize(float lifeTime, Vector3 direction)
    {
        _currentLifetime = lifeTime;

        SetDirection(direction);

        _isInitialized = true;
    }

    private void SetDirection(Vector3 direction)
    {
        _direction = direction;
        _direction.Normalize();
    }

    public void UpdateIndicator(Vector3 normalizedPlayerDirection, Vector3 normalizedUpVector, float elapsedTime)
    {
        if (!_isInitialized)
        {
            return;
        }

        _currentLifetime -= elapsedTime;

        float angleBetweenVectors = CalculateRotationAngleDifference(normalizedPlayerDirection, normalizedUpVector);
        ApplyRotation(angleBetweenVectors);
    }

    private float CalculateRotationAngleDifference(Vector3 normalizedPlayerLookAtDirection, Vector3 normalizedUpVector)
    {
        Vector2 playerDirection2 = new Vector2(normalizedPlayerLookAtDirection.x, normalizedPlayerLookAtDirection.z);
        playerDirection2.Normalize();

        Vector2 direction2 = new Vector2(_direction.x, _direction.z);
        direction2.Normalize();

        float dotProduct = Vector2.Dot(playerDirection2,direction2);
        float angleInRadians = Mathf.Acos(dotProduct);

        Vector3 crossProduct = Vector3.Cross(normalizedPlayerLookAtDirection, _direction);
        if(Vector3.Dot(crossProduct, normalizedUpVector) > 0)
        {
            angleInRadians = -angleInRadians;
        }

        return angleInRadians * Mathf.Rad2Deg;
    }

    private void ApplyRotation(float angleBetweenVectors)
    {
        Vector3 currentRotationEulerAngles = transform.rotation.eulerAngles;
        currentRotationEulerAngles.z = angleBetweenVectors;
        transform.rotation = Quaternion.Euler(currentRotationEulerAngles);
    }
}