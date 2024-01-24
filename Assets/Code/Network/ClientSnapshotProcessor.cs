using GONet;
using System;
using System.Collections.Generic;

/// <summary>
/// Process a server snapshot and distributes its information along the target entities
/// </summary>
[Obsolete("Going pure GONet.")]
public class ClientSnapshotProcessor
{
    private readonly List<GONetParticipant> _allEnabledGNPs = new List<GONetParticipant>();
    
    internal void TrackGONetParticipant(GONetParticipant gonetParticipant)
    {
        _allEnabledGNPs.Add(gonetParticipant);
    }

    internal void OnGONetParticipantDisabled(GONetParticipant gonetParticipant)
    {
        _allEnabledGNPs.Remove(gonetParticipant);
    }
}
