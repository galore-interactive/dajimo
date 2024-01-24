using MemoryPack;
using GONet;
using UnityEngine;

[MemoryPackable]
public partial class SnapshotState : ITransientEvent
{
    public bool hasPlayerState;
    public IPlayerState playerState;
    public float serverTime;
    public uint serverTick;
    public ushort targetClientAuthorityId;

    public SnapshotState(bool hasPlayerState, IPlayerState playerState, float serverTime, uint serverTick, ushort targetClientAuthorityId)
    {
        this.hasPlayerState = hasPlayerState;
        this.playerState = playerState;
        this.serverTime = serverTime;
        this.serverTick = serverTick;
        this.targetClientAuthorityId = targetClientAuthorityId;
    }

    [MemoryPackIgnore]
    public long OccurredAtElapsedTicks => 0;

    public override string ToString()
    {
        return JsonUtility.ToJson(this);
    }
}
