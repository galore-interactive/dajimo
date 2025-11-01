using UnityEngine;
using Unity.Cinemachine;
using GONet;

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

        GONetLog.Debug($"[PlayerCameraController] Awake: Found virtual camera '{_virtualCamera?.name}', switcher={_cinemachineCameraSwitcher != null}");

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
        GONetLog.Debug($"[PlayerCameraController] SetCameraTarget called with target: {target?.name}, position: {target?.position}");
        SetVirtualCameraFollow(target);
        // For FPS camera: Don't set LookAtTarget, let the camera rotate with the tracking target
        GONetLog.Debug($"[PlayerCameraController] After setting target - TrackingTarget: {_virtualCamera.Target.TrackingTarget?.name}");
    }

    private void SetVirtualCameraFollow(Transform target)
    {
        _virtualCamera.Target.TrackingTarget = target;
    }

    public void SetCameraAsMain()
    {
        const string PLAYER_CAMERA_NAME = "PlayerCamera";

        if (_cinemachineCameraSwitcher == null)
        {
            GONetLog.Error("[PlayerCameraController] CinemachineCameraSwitcher is null! Cannot switch camera.");
            return;
        }

        if (_virtualCamera == null)
        {
            GONetLog.Error("[PlayerCameraController] Virtual camera is null! Cannot switch camera.");
            return;
        }

        GONetLog.Debug($"[PlayerCameraController] SetCameraAsMain called. VCam: {_virtualCamera.name}, Priority before: {_virtualCamera.Priority.Value}");

        if(!_cinemachineCameraSwitcher.IsCameraTracked(PLAYER_CAMERA_NAME))
        {
            _cinemachineCameraSwitcher.AddCamera(PLAYER_CAMERA_NAME, _virtualCamera);
            GONetLog.Debug($"[PlayerCameraController] Added camera '{PLAYER_CAMERA_NAME}' to switcher");
        }

        _cinemachineCameraSwitcher.SwitchCamera(PLAYER_CAMERA_NAME);
        GONetLog.Debug($"[PlayerCameraController] Switched to camera '{PLAYER_CAMERA_NAME}', Priority after: {_virtualCamera.Priority.Value} (Enabled: {_virtualCamera.Priority.Enabled})");
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
