using GONet;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ServerSimulator
{
    public readonly HashSet<INetworkEntity> networkEntitiesWithUnsetGONetId = new();

    private readonly Dictionary<uint, INetworkEntity> _activeNetworkEntitiesByGONetId = new();
    public IReadOnlyDictionary<uint, INetworkEntity> ActiveNetworkEntitiesByGONetId => _activeNetworkEntitiesByGONetId;

    private readonly Dictionary<ushort, HashSet<INetworkEntity>> _activeNetworkEntitiesByAuthorityId = new();
    public IReadOnlyDictionary<ushort, HashSet<INetworkEntity>> ActiveNetworkEntitiesByAuthorityId => _activeNetworkEntitiesByAuthorityId;

    /// <summary>
    /// try get by auth id whether or not it is active of inactive currently
    /// </summary>
    public bool TryGetPlayerEntityForAuthorityId<TPlayerEntity>(ushort authorityId, out TPlayerEntity typed) 
        where TPlayerEntity : class, IPlayerNetworkEntity
    {
        if (_activeNetworkEntitiesByAuthorityId.TryGetValue(authorityId, out HashSet<INetworkEntity> playerNetworkEntities))
        {
            foreach (INetworkEntity playerNetworkEntity in playerNetworkEntities)
            {
                if (playerNetworkEntity is TPlayerEntity)
                {
                    typed = playerNetworkEntity as TPlayerEntity;
                    return true;
                }
            }
        }

        typed = default;
        return false;
    }

    /// <summary>
    /// Adds a network entity to be tracked in the simulation. If you add a player network entity you don't need to call to <c>AddNetworkEntity</c> as it is automatically added too.
    /// </summary>
    /// <param name="networkEntity">The network entity</param>
    public void AddNetworkEntity(INetworkEntity networkEntity)
    {
        ushort clientAuthorityId = networkEntity.GetClientOwnerId();
        if (!_activeNetworkEntitiesByAuthorityId.TryGetValue(clientAuthorityId, out HashSet<INetworkEntity> networkEntities))
        {
            _activeNetworkEntitiesByAuthorityId[clientAuthorityId] = networkEntities = new();
        }
        networkEntities.Add(networkEntity);

        _activeNetworkEntitiesByGONetId[networkEntity.GetNetworkEntityId()] = networkEntity;
    }

    /// <summary>
    /// Removes a network entity from the simulation.
    /// </summary>
    /// <param name="networkEntityId">The ID of the network entity to remove</param>
    public void RemoveNetworkEntity(uint networkEntityId, bool shouldDestroyFinally = false)
    {
        if (_activeNetworkEntitiesByGONetId.ContainsKey(networkEntityId))
        {
            _activeNetworkEntitiesByGONetId.Remove(networkEntityId);
        }
        else
        {
            Debug.LogWarning("You are trying to remove a Network Entity that does not exist in the active entities dictionary!");
        }

        foreach (var kvp in _activeNetworkEntitiesByAuthorityId)
        {
            HashSet<INetworkEntity> entitiesForAuthorityId = kvp.Value;
            INetworkEntity match = entitiesForAuthorityId.FirstOrDefault(x => x.GetNetworkEntityId() == networkEntityId);
            if (match != default)
            {
                entitiesForAuthorityId.Remove(match);
                
                // ASSuME an entity only belongs to one authorityId and exit now
                break;
            }
        }

        if (shouldDestroyFinally)
        {
            GameObject.Destroy(GONetMain.gonetParticipantByGONetIdMap[networkEntityId].gameObject);
        }
    }

    internal void OnGONetIdChanged(GONetEventEnvelope<SyncEvent_ValueChangeProcessed> eventEnvelope)
    {
        _activeNetworkEntitiesByGONetId.Remove(eventEnvelope.Event.ValuePrevious.System_UInt32);
        _activeNetworkEntitiesByGONetId[eventEnvelope.Event.ValueNew.System_UInt32] = eventEnvelope.GONetParticipant?.GetComponent<INetworkEntity>();
    }
}
