using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button _startClientButton; // aka join room
    [SerializeField] private Button _startServerButton;
    [SerializeField] private Button exitButton; // context sensitive (exit game on start/join screen, exit room back to start/join screen in-game)
    
    [SerializeField] private NetworkInitializer _networkInitializer;
    [SerializeField] private GameObject _startNetworkUIScreen;

    [SerializeField] private CinemachineVirtualCamera _menuVirtualCamera;
    private CinemachineCameraSwitcher _cinemachineCameraSwitcher;
    private NetworkController _networkController;

    [SerializeField] private GameObject _mobileControlsUI;

    private void Awake()
    {
        _networkController = FindObjectOfType<NetworkController>();
        _cinemachineCameraSwitcher = FindObjectOfType<CinemachineCameraSwitcher>();
        _mobileControlsUI.SetActive(false);

#if UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX || ENABLE_IL2CPP
        _startClientButton.GetComponentInChildren<TMP_Text>().text = "Join";

        _startServerButton.interactable = false;
        _startServerButton.gameObject.SetActive(false);
#endif

#if UNITY_ANDROID || UNITY_IOS
        exitButton.onClick.AddListener(Exit);
#else
        exitButton.gameObject.SetActive(false);
#endif

    }

    private void Exit()
    {
        if (_startNetworkUIScreen.activeInHierarchy)
        {
            Application.Quit();
        }
        else
        { // ASSuME client!!!
            _networkInitializer.DisconnectClient();
            StopGame(); // NOTE: if the above call ended up causing _networkController.OnLocalDisconnect to fire, this would not be necessary
        }
    }

    private void Start()
    {
        const string MC = "MenuCamera";
        _cinemachineCameraSwitcher.AddCamera(MC, _menuVirtualCamera);
        _cinemachineCameraSwitcher.SwitchCamera(MC);
    }

    private void OnEnable()
    {
        _startClientButton.onClick.AddListener(StartClient);
        _startServerButton.onClick.AddListener(StartServer);
        _networkController.OnLocalDisconnect += StopGame;

        _networkInitializer.OnHostStart += StartGame;
        _networkInitializer.OnClientStart += StartGame;
        _networkInitializer.OnServerStart += StartGame;
    }

    private void OnDisable()
    {
        _startClientButton.onClick.RemoveListener(StartClient);
        _startServerButton.onClick.RemoveListener(StartServer);
        _networkController.OnLocalDisconnect -= StopGame;

        _networkInitializer.OnHostStart -= StartGame;
        _networkInitializer.OnClientStart -= StartGame;
        _networkInitializer.OnServerStart -= StartGame;
    }

    private void StartClient()
    {
        _networkInitializer.StartClient();
    }

    private void StartServer()
    {
        _networkInitializer.StartServer();
    }

    private void StartGame()
    {
        _startNetworkUIScreen.SetActive(false);

#if !UNITY_EDITOR || (!UNITY_ANDROID && !UNITY_IOS)
        LockCursor();
#endif

#if UNITY_ANDROID || UNITY_IOS
        _mobileControlsUI.SetActive(true);
#endif
    }

    private void StopGame()
    {
        _startNetworkUIScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        _mobileControlsUI.SetActive(false);
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
}
