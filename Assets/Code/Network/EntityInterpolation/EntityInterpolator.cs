using System;
using System.Collections.Generic;
using UnityEngine;

public class EntityInterpolator : IEntityInterpolator
{
    public static event Action OnEmptyBuffer;

    public const int INTERPOLATION_DELAY_TICKS = 2;
    private readonly float _interpolationDelayTime;
    private List<EntityInterpolationData> _entityStatesBuffer;

    EntityInterpolationData beforeState;
    EntityInterpolationData afterState;

    private bool _shouldInterpolate = false;
    private uint _tickRate;
    private int _extrapolatedStatesCount = 0;

    private bool _isInitialized = false;

    public EntityInterpolator(Vector3 initialPosition, Vector3 initialCameraLookAtEulerAngles, uint tickRate)
    {
        _tickRate = tickRate;
        _interpolationDelayTime = INTERPOLATION_DELAY_TICKS * (1f / _tickRate);
        _entityStatesBuffer = new List<EntityInterpolationData>();

        EntityState s = new EntityState(0, Vector2.zero, initialPosition, initialCameraLookAtEulerAngles, Vector3.zero, true, false);
        beforeState = new EntityInterpolationData();
        afterState = new EntityInterpolationData(s, 0f);
    }

    public void AddEntityState(EntityState entityState, float time)
    {
        if (!IsStateValid(time))
        {
            return;
        }

        EntityInterpolationData entityInterpolationData = new EntityInterpolationData(entityState, time);
        AddToBuffer(entityInterpolationData);

        if(!_shouldInterpolate && _entityStatesBuffer.Count >= INTERPOLATION_DELAY_TICKS)
        {
            if(!_isInitialized)
            {
                _isInitialized = true;
            }
            _shouldInterpolate = true;
        }
    }

    private bool IsStateValid(float time)
    {
        if(afterState.time < time)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void AddToBuffer(EntityInterpolationData entityInterpolationData)
    {
        if(_entityStatesBuffer.Count == 0 || entityInterpolationData.time > _entityStatesBuffer[_entityStatesBuffer.Count - 1].time)
        {
            _entityStatesBuffer.Add(entityInterpolationData);
        }
        else
        {
            int insertIndex = -1;

            for (int i = 0; i < _entityStatesBuffer.Count; i++)
            {
                if(entityInterpolationData.time < _entityStatesBuffer[i].time)
                {
                    insertIndex = i;
                    break;
                }
            }

            _entityStatesBuffer.Insert(insertIndex, entityInterpolationData);
        }
    }

    private EntityInterpolationData GetNextBufferState()
    {
        EntityInterpolationData nextBufferState = _entityStatesBuffer[0];
        _entityStatesBuffer.RemoveAt(0);
        return nextBufferState;
    }

    public EntityState InterpolateEntity(float currentTime)
    {
        if(!_isInitialized)
        {
            return new EntityState();
        }

        //Calculate the interpolatedTime in the past
        float interpolatedTime = currentTime - _interpolationDelayTime;

        if(afterState.time < interpolatedTime)
        {
            bool hasNewAfterState = false;

            while(!hasNewAfterState)
            {
                EntityInterpolationData newData = GetNextEntityState();
                beforeState = afterState;
                afterState = newData;

                if (newData.time > interpolatedTime)
                {
                    hasNewAfterState = true;
                }
            }
        }

        //Calculate the normalized time fraction of the progress that has occur between the two states
        float normalizedTimeFraction = (interpolatedTime - beforeState.time) / (afterState.time - beforeState.time);

        //Calculate the new interpolated values
        Vector3 newPosition = Vector3.Lerp(beforeState.entityState.position, afterState.entityState.position, normalizedTimeFraction);
        Vector3 cameraLookAtEulerAngles = LerpRotation(beforeState.entityState.cameraLookAtEulerAngles, afterState.entityState.cameraLookAtEulerAngles, normalizedTimeFraction);
        Vector2 movementDirection = Vector2.Lerp(beforeState.entityState.movementInput, afterState.entityState.movementInput, normalizedTimeFraction);

        return new EntityState(afterState.entityState.networkObjectID, movementDirection, newPosition, cameraLookAtEulerAngles, beforeState.entityState.velocityVector, beforeState.entityState.isGrounded, beforeState.entityState.isCrouched);
    }

    private Vector3 LerpRotation(Vector3 beforeStateEulerAngles, Vector3 afterStateEulerAngles, float normalizedTimeFraction)
    {
        return new Vector3(Mathf.LerpAngle(beforeStateEulerAngles.x, afterStateEulerAngles.x, normalizedTimeFraction),
                           Mathf.LerpAngle(beforeStateEulerAngles.y, afterStateEulerAngles.y, normalizedTimeFraction),
                           Mathf.LerpAngle(beforeStateEulerAngles.z, afterStateEulerAngles.z, normalizedTimeFraction));
    }
    
    private EntityInterpolationData GetNextEntityState()
    {
        _shouldInterpolate = CheckIfShouldInterpolate();

        if(_shouldInterpolate)
        {
            _extrapolatedStatesCount = 0;
            EntityInterpolationData d = GetNextBufferState();
            //Debug.Log($"Interpolated state: {d.entityState.position}");
            return d;
        }
        else
        {
            _extrapolatedStatesCount++;

            if (_extrapolatedStatesCount == 1)
            {
                OnEmptyBuffer?.Invoke();
                Debug.LogWarning("Entity interpolation Buffer is empty. Starting Extrapolation...");
            }

            EntityInterpolationData d = GetNewStateFromExtrapolation(afterState, beforeState);
            //Debug.Log($"Before state: {beforeState.entityState.position} | {beforeState.time}, After state: {afterState.entityState.position} | {afterState.time}");
            //Debug.Log($"Extrapolated state: {d.entityState.position}");

            if(d.entityState.position.x < -30f || d.entityState.position.x > 30f || d.entityState.position.z < -30 || d.entityState.position.z > 30)
            {
                Debug.LogWarning("MUY LEJOSS");
                Debug.Log("MUY LEJOSS");
                Debug.Log($"Before state: {beforeState.entityState.position} | {beforeState.time}, After state: {afterState.entityState.position} | {afterState.time}. Extrapolated Count: {_extrapolatedStatesCount}");
                Debug.Log($"Extrapolated state: {d.entityState.position}");
            }

            return d;
        }
    }

    private bool CheckIfShouldInterpolate()
    {
        if(_entityStatesBuffer.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private EntityInterpolationData GetNewStateFromExtrapolation(EntityInterpolationData currentState, EntityInterpolationData beforeState)
    {
        Vector3 velocity = currentState.entityState.velocityVector;
        if(currentState.entityState.isGrounded)
        {
            velocity.y = 0f;
        }

        float timeIncrement = 1f / _tickRate;
        float extrapolatedStateTime = currentState.time + timeIncrement;

        Vector3 extrapolatedPosition = CalculateExtrapolatedPosition(currentState.entityState.position, currentState.entityState.velocityVector, timeIncrement);
        return new EntityInterpolationData(new EntityState(currentState.entityState.networkObjectID, currentState.entityState.movementInput, extrapolatedPosition, currentState.entityState.cameraLookAtEulerAngles, currentState.entityState.velocityVector, currentState.entityState.isGrounded, currentState.entityState.isCrouched), extrapolatedStateTime);
    }

    private Vector3 CalculateVelocityFromTwoPositions(Vector3 finalPosition, Vector3 initialPosition, float finalTime, float initialTime)
    {
        return (finalPosition - initialPosition) / (finalTime - initialTime);
    }

    private Vector3 CalculateExtrapolatedPosition(Vector3 currentPosition, Vector3 currentVelocity, float timeIncrement)
    {
        return currentPosition + (currentVelocity * timeIncrement);
    }
}
