using UnityEngine;
using Cinemachine;

[DisallowMultipleComponent]
[RequireComponent(typeof(CinemachineVirtualCamera))]
public class PlayerCameraController : MonoBehaviour
{
    private CinemachineVirtualCamera _virtualCamera;
    private CinemachineCameraSwitcher _cinemachineCameraSwitcher;
    private CinemachineCameraShaker _cameraShaker;
    [SerializeField] private CinemachineCameraShakeConfiguration _shakeConfiguration;

    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _cinemachineCameraSwitcher = FindObjectOfType<CinemachineCameraSwitcher>();
        _cameraShaker = new CinemachineCameraShaker(_virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>());
    }

    public void SetCameraTarget(Transform target)
    {
        SetVirtualCameraFollow(target);
        SetVirtualCameraLookAt(target);
    }

    private void SetVirtualCameraFollow(Transform target)
    {
        _virtualCamera.Follow = target;
    }
    private void SetVirtualCameraLookAt(Transform target)
    {
        _virtualCamera.LookAt = target;
    }

    public void SetCameraAsMain()
    {
        const string PLAYER_CAMERA_NAME = "PlayerCamera";
        if(!_cinemachineCameraSwitcher.IsCameraTracked(PLAYER_CAMERA_NAME))
        {
            _cinemachineCameraSwitcher.AddCamera(PLAYER_CAMERA_NAME, _virtualCamera);
        }

        _cinemachineCameraSwitcher.SwitchCamera(PLAYER_CAMERA_NAME);
    }

    private void Update()
    {
        _cameraShaker.Update();
    }

    public void ShakeCamera(float duration)
    {
        _cameraShaker.ShakeCamera(duration);
    }
}
