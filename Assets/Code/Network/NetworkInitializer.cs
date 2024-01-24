using System;
using UnityEngine;
using System.Collections.Generic;
using GONet;
using System.Collections;

public class NetworkInitializer : MonoBehaviour
{
    public event Action OnHostStart;
    public event Action OnClientStart;
    public event Action OnServerStart;

    [SerializeField] private ChristiansClockSyncronizator _christiansClockSyncronizatorPrefab;
    private readonly ushort _port = 7777;
    private readonly string _iP = "127.0.0.1";
    private readonly int MAX_CLIENT_COUNT_PER_SERVER = 100;
    private HashSet<Renderer> _renderers;

    private void Awake()
    {
        _renderers = new HashSet<Renderer>(FindObjectsOfType<Renderer>(true));

        if (GONetMain.IsServer)
        { 
            // if we know it is a server already, must be because of command line args...dont wait for a ui button click
            // that will never come....start server now!
            StartServer();
        }
    }

    public void StartClient()
    {
        GONetMain.GONetClient = new GONetClient(new NetcodeIO.NET.Client());
        
        GONetGlobal.ServerIPAddress_Actual = GONetGlobal.ServerIPAddress_Default;
        GONetGlobal.ServerPort_Actual = GONetGlobal.ServerPort_Default;
        GONetMain.GONetClient.ConnectToServer(GONetGlobal.ServerIPAddress_Actual, GONetGlobal.ServerPort_Actual, 30);

        bool succesfullyStarted = true;
        if (succesfullyStarted)
        {
            Debug.Log("You joined as Client");
            OnClientStart?.Invoke();
        }
    }

    public void DisconnectClient()
    {
        if (GONetMain.IsClient)
        {
            GONetMain.GONetClient.Disconnect();
        }
    }

    public void StartServer()
    {
        GONetGlobal.ServerIPAddress_Actual = GONetGlobal.ServerIPAddress_Default;
        GONetGlobal.ServerPort_Actual = GONetGlobal.ServerPort_Default;
        GONetMain.gonetServer = new GONetServer(MAX_CLIENT_COUNT_PER_SERVER, GONetGlobal.ServerIPAddress_Actual, GONetGlobal.ServerPort_Actual);

        GONetMain.gonetServer.Start();

        bool succesfullyStarted = true;
        if (succesfullyStarted)
        {
            Debug.Log("You joined as Server");
            DeactivateObjectVisuals();
            OnServerStart?.Invoke();
        }
    }

    private void DeactivateObjectVisuals()
    {
        foreach(Renderer renderer in _renderers)
        {
            renderer.enabled = false;
        }
    }
}