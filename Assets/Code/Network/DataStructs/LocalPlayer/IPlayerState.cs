using GONet;
using MemoryPack;
using UnityEngine;

[MemoryPack.MemoryPackUnion(2001, typeof(BipedPlayerState))]
[MemoryPack.MemoryPackUnion(2002, typeof(DronePlayerState))]
[MemoryPackable]
public partial interface IPlayerState : IGONetEvent
{
    public PlayableEntityType PlayableEntityType { get; }
    public void GetShotPointAndShotDirection(out Vector3 shotPoint, out Vector3 shotLookAtDirection);
    public uint ClientTick { get; }
    public bool AreNearlyEqual(IPlayerState otherPlayerState, float tolerance);
}