using System.Collections.Generic;
using System.Diagnostics;
using GONet;

public class SnapshotGenerator
{
    /// <summary>
    /// Generates a custom client snapshot base on the current world state
    /// </summary>
    /// <param name="hasPlayerState">If the target client has generated a playerstate during the current tick</param>
    /// <param name="playerState">The client's local playerstate</param>
    /// <param name="serverTime">The current server time</param>
    /// <param name="tick">The current tick</param>
    /// <returns></returns>
    public SnapshotState GenerateClientSnapshot(bool hasPlayerState, IPlayerState playerState, float serverTime, uint tick, ushort targetClientAuthorityId)
    {

        return new SnapshotState(hasPlayerState, playerState, serverTime, tick, targetClientAuthorityId);
    }
}
