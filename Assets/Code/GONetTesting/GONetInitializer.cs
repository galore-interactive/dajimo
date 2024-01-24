using GONet;
using UnityEngine;

public class GONetInitializer : MonoBehaviour
{
    [SerializeField] private bool _isServer;
    private GONetRPCHandler _rpcHandler;
    private uint _snapshotIndex = 0;
    private int _inputStateIndex = 0;

    private void Awake()
    {
        _rpcHandler = new GONetRPCHandler();
    }

    public void InitClient()
    {
        _isServer = false;
        GONetGlobal.ServerIPAddress_Actual = GONetGlobal.ServerIPAddress_Default;
        GONetGlobal.ServerPort_Actual = GONetGlobal.ServerPort_Default;

        GONetMain.GONetClient = new GONetClient(new NetcodeIO.NET.Client());
        GONetMain.GONetClient.ConnectToServer(GONetGlobal.ServerIPAddress_Actual, GONetGlobal.ServerPort_Actual, 30);
    }

    public void DisconnectClient()
    {
        if(GONetMain.IsClient)
        {
            GONetMain.GONetClient.Disconnect();
        }
    }

    public void InitServer()
    {
        _isServer = true;
        GONetGlobal.ServerIPAddress_Actual = GONetGlobal.ServerIPAddress_Default;
        GONetGlobal.ServerPort_Actual = GONetGlobal.ServerPort_Default;

        GONetMain.gonetServer = new GONetServer(10, GONetGlobal.ServerIPAddress_Actual, GONetGlobal.ServerPort_Actual);
        GONetMain.gonetServer.Start();
    }

    public void StopServer()
    {
        if(GONetMain.IsServer)
        {
            Debug.LogWarning("STOPPING SERVER...");
            GONetMain.gonetServer.Stop();
        }
    }

    private void Update()
    {
        if(GONetMain.IsServer)
        {
            Debug.Log("I am a server");
        }
    }
}
