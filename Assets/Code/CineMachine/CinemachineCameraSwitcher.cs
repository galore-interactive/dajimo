using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Cinemachine;

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

        SetCameraPriority(camera, ACTIVE_CAMERA_PRIORITY);
        _activeCameraName = newCameraName;

        foreach(KeyValuePair<string, CinemachineCamera> cameraData in _trackedCamerasToNames)
        {
            if(!_activeCameraName.Equals(cameraData.Key))
            {
                SetCameraPriority(cameraData.Value, 0);
            }
        }
    }

    private void SetCameraPriority(CinemachineCamera camera, int priority)
    {
        camera.Priority = priority;
    }
}