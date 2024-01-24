using System.Collections.Generic;
using GONet;

/// <summary>
/// Used on server and client sides.
/// </summary>
public class ServerEntityEventsHolder
{
    public Dictionary<uint, List<LocalContextEntityEvent>> entityEventsWithUnsetGONetId;
    private readonly Dictionary<uint, List<LocalContextEntityEvent>> _entityEvents;

    public ServerEntityEventsHolder()
    {
        entityEventsWithUnsetGONetId = new Dictionary<uint, List<LocalContextEntityEvent>>();
        _entityEvents = new Dictionary<uint, List<LocalContextEntityEvent>>();
    }

    public void Initialize()
    {
        if (GONetMain.IsClient)
        {
            GONetMain.EventBus.Subscribe<LocalContextEntityEvent>(Client_OnEntityEventArrived, (e) => e.IsSourceRemote);
        }
        else if (GONetMain.IsServer)
        {
            //GONetMain.EventBus.Subscribe<EntityEvent>(Server_OnEntityEventArrived, (e) => e.IsSourceRemote);
        }
    }

    private void Server_OnEntityEventArrived(GONetEventEnvelope<LocalContextEntityEvent> eventEnvelope)
    {
        GONetLog.Debug("Entity event received.");
    }

    private void Client_OnEntityEventArrived(GONetEventEnvelope<LocalContextEntityEvent> eventEnvelope)
    {
        //GONetLog.Debug($"Entity event received.  type: {eventEnvelope.Event.type}");

        if (GONetMain.gonetParticipantByGONetIdMap.ContainsValue(eventEnvelope.GONetParticipant))
        {
            eventEnvelope.GONetParticipant.gameObject.GetComponent<INetworkEntity>().ReceiveEntityEvent(eventEnvelope.Event);
        }
        else
        {
            GONetLog.Error($"[OnEntityEventArrived]: The entity with NetworkObjectId = {eventEnvelope.Event.GONetId} could not be found. Event type: {eventEnvelope.Event.type}.  event.GNP: {eventEnvelope.GONetParticipant}");
        }
    }

    public void AddEvent(uint entityId, LocalContextEntityEvent entityEvent)
    {
        if (!_entityEvents.TryGetValue(entityId, out List<LocalContextEntityEvent> entityEvents))
        {
            _entityEvents[entityId] = entityEvents = new List<LocalContextEntityEvent>();
        }
        entityEvents.Add(entityEvent);
    }

    public void PublishCurrentTickEvents()
    {
        foreach (List<LocalContextEntityEvent> entityEvents in _entityEvents.Values)
        {
            foreach (LocalContextEntityEvent entityEvent in entityEvents)
            {
                GONetMain.EventBus.Publish(entityEvent, shouldPublishReliably: true);
            }
            entityEvents.Clear();
        }
    }
}
