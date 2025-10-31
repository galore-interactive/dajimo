using UnityEngine;
using Unity.Cinemachine;

[DisallowMultipleComponent]
[RequireComponent(typeof(CinemachineCamera))]
public class PlayerCameraController : MonoBehaviour
{
    private CinemachineCamera _virtualCamera;
    private CinemachineCameraSwitcher _cinemachineCameraSwitcher;
    private CinemachineCameraShaker _cameraShaker;
    [SerializeField] private CinemachineCameraShakeConfiguration _shakeConfiguration;

    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineCamera>();
        _cinemachineCameraSwitcher = FindObjectOfType<CinemachineCameraSwitcher>();

        // Cinemachine 3.x: GetComponent returns CinemachineComponentBase, need to get or add the noise component
        var noiseComponent = _virtualCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
        if (noiseComponent == null)
        {
            // Add the component if it doesn't exist
            noiseComponent = _virtualCamera.gameObject.AddComponent<CinemachineBasicMultiChannelPerlin>();
        }
        _cameraShaker = new CinemachineCameraShaker(noiseComponent);
    }

    public void SetCameraTarget(Transform target)
    {
        SetVirtualCameraFollow(target);
        SetVirtualCameraLookAt(target);
    }

    private void SetVirtualCameraFollow(Transform target)
    {
        _virtualCamera.Target.TrackingTarget = target;
    }
    private void SetVirtualCameraLookAt(Transform target)
    {
        _virtualCamera.Target.CustomLookAtTarget = true;
        _virtualCamera.Target.LookAtTarget = target;
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
