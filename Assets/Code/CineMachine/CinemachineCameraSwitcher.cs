using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Cinemachine;
using GONet;

public class CinemachineCameraSwitcher : MonoBehaviour
{
    private IDictionary<string, CinemachineCamera> _trackedCamerasToNames;
    private string _activeCameraName;
    private const int ACTIVE_CAMERA_PRIORITY = 20;

    private void Awake()
    {
        _activeCameraName = System.String.Empty;
        _trackedCamerasToNames = new Dictionary<string, CinemachineCamera>();
    }

    public void AddCamera(string name, CinemachineCamera cameraToTrack)
    {
        Assert.IsNotNull(cameraToTrack, $"[{this.GetType().Name} at TrackCamera]: The camera called {name} is null");
        Assert.IsFalse(IsCameraTracked(name), $"[{this.GetType().Name} at TrackCamera]: There is already a camera called {name}");

        SetCameraPriority(cameraToTrack, 0);

        bool addedSuccesfully = _trackedCamerasToNames.TryAdd(name, cameraToTrack);
        Assert.IsTrue(addedSuccesfully, $"[{this.GetType().Name} at TrackCamera]: The camera to track could not be added");
    }

    public bool IsCameraTracked(string cameraName)
    {
        return _trackedCamerasToNames.ContainsKey(cameraName);
    }

    public void SwitchCamera(string newCameraName)
    {
        CinemachineCamera camera;
        bool foundSuccesfully = _trackedCamerasToNames.TryGetValue(newCameraName, out camera);
        Assert.IsTrue(foundSuccesfully, $"[{this.GetType().Name} at SwitchCamera]: There are no tracked cameras called: {newCameraName}");

        GONetLog.Debug($"[CinemachineCameraSwitcher] Switching to camera: {newCameraName}");

        SetCameraPriority(camera, ACTIVE_CAMERA_PRIORITY);
        _activeCameraName = newCameraName;

        foreach(KeyValuePair<string, CinemachineCamera> cameraData in _trackedCamerasToNames)
        {
            if(!_activeCameraName.Equals(cameraData.Key))
            {
                GONetLog.Debug($"[CinemachineCameraSwitcher] Disabling camera: {cameraData.Key}, setting priority to 0");
                SetCameraPriority(cameraData.Value, 0);
            }
        }

        GONetLog.Debug($"[CinemachineCameraSwitcher] Active camera is now: {_activeCameraName} with priority {ACTIVE_CAMERA_PRIORITY}");
    }

    private void SetCameraPriority(CinemachineCamera camera, int priority)
    {
        GONetLog.Debug($"[CinemachineCameraSwitcher] SetCameraPriority BEFORE: camera={camera.name}, priority to set={priority}, current priority={camera.Priority.Value}, enabled={camera.Priority.Enabled}");

        // Cinemachine 3.x: Must create new PrioritySettings struct and assign it
        camera.Priority = new Unity.Cinemachine.PrioritySettings
        {
            Enabled = true,
            Value = priority
        };

        GONetLog.Debug($"[CinemachineCameraSwitcher] SetCameraPriority AFTER assignment: priority={camera.Priority.Value}, enabled={camera.Priority.Enabled}");

        // Ensure camera component is enabled
        camera.enabled = true;

        GONetLog.Debug($"[CinemachineCameraSwitcher] SetCameraPriority FINAL: camera.enabled={camera.enabled}, priority={camera.Priority.Value}");
    }
}