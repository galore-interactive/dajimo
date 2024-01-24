using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class CameraShaker : MonoBehaviour
{
    private HashSet<CameraShakeEvent> _activeShakeEvents = new HashSet<CameraShakeEvent>();

    public void AddShakeEvent(CameraShakeConfigurationSO shakeEventConfiguration)
    {
        CameraShakeEvent shakeEvent = new CameraShakeEvent(shakeEventConfiguration);
        _activeShakeEvents.Add(shakeEvent);
    }

    private void LateUpdate()
    {
        Vector3 transationalShaking = Vector3.zero;
        Vector3 rotationalShaking = Vector3.zero;

        ISet<CameraShakeEvent> deadShakeEvents = new HashSet<CameraShakeEvent>();

        foreach(CameraShakeEvent shakeEvent in _activeShakeEvents)
        {
            shakeEvent.Update();

            transationalShaking += shakeEvent.TranslationalShake;
            rotationalShaking += shakeEvent.RotationalShake;

            if (!shakeEvent.IsAlive())
            {
                deadShakeEvents.Add(shakeEvent);
            }
        }

        transform.localPosition = transationalShaking;
        transform.localEulerAngles = rotationalShaking;

        RemoveDeadShakeEvents(deadShakeEvents);
    }

    private void RemoveDeadShakeEvents(ISet<CameraShakeEvent> shakeEventsToRemove)
    {
        foreach(CameraShakeEvent shakeEvent in shakeEventsToRemove)
        {
            _activeShakeEvents.Remove(shakeEvent);
        }
    }
}
