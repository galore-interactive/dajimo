using Hathora.Demos.Shared.Scripts.Common;
using UnityEngine;

namespace GONet.Hathora
{
    /// <summary>
    /// Commandline helper - run via `-mode {server|client|host}`.
    /// </summary>
    [RequireComponent(typeof(NetworkInitializer))]
    public class HathoraGONetArgHandler : HathoraArgHandlerBase
    {
        NetworkInitializer networkInitializer;

        private void Awake()
        {
            networkInitializer = GetComponent<NetworkInitializer>();
        }

        private void Start() => 
            _ = base.InitArgsAsync();

        protected override void ArgModeStartServer()
        {
            base.ArgModeStartServer();

            if (GONetMain.IsServer && GONetMain.gonetServer.IsRunning)
                return;

            Debug.Log($"[{GetType().Name}] Starting GONet Server ...");
            networkInitializer.StartServer();
        }

        protected override void ArgModeStartClient()
        {
            base.ArgModeStartClient();

            if (GONetMain.IsClient && GONetMain.GONetClient.IsConnectedToServer)
                return;
            
            Debug.Log($"[{GetType().Name}] Starting Client ...");
            GONetStateMgr.Singleton.StartClient();
        }

        protected override void ArgModeStartHost()
        {
            base.ArgModeStartHost();
            
            Debug.LogWarning($"[{GetType().Name}] Doing nothing.  No start host support.");
        }
    }
}
