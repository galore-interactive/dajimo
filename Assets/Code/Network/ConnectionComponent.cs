using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using GONet;

public class ConnectionComponent
{
    public event Action OnLocalDisconnect;

    /// <summary>
    /// Only for <see cref="GetConnectedClientAuthorityIds"/>  optimization purposes. Don't use it anywhere else.
    /// </summary>
    private readonly List<ushort> _remoteConnectedClientAuthorityIds;

    public ConnectionComponent()
    {
        _remoteConnectedClientAuthorityIds = new List<ushort>();
    }

    public IReadOnlyList<ushort> GetConnectedClientAuthorityIds()
    {
        _remoteConnectedClientAuthorityIds.Clear();

        List<GONetRemoteClient> remoteConnectedClients = GONetMain.gonetServer.remoteClients;
        foreach(GONetRemoteClient remoteClient in remoteConnectedClients)
        {
            _remoteConnectedClientAuthorityIds.Add(remoteClient.ConnectionToClient.OwnerAuthorityId);
        }
        return _remoteConnectedClientAuthorityIds;
    }

    public void DisconnectClient(ushort clientAuthorityId)
    {
        if (GONetMain.IsServer)
        {
            RemoveClient(clientAuthorityId);
        }

        if (GONetMain.IsClient)
        {
            OnLocalDisconnect?.Invoke();
        }
    }

    private void RemoveClient(ushort clientAuthorityId)
    {
        Assert.IsTrue(GONetMain.IsServer);
    }

    //This method is called when the own machine asks for a disconnection of itself
    public IEnumerator DisconnectLocal()
    {
        yield return null;
    }
}
