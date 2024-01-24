using GONet;
using MemoryPack;

[MemoryPack.MemoryPackUnion(1001, typeof(BipedInputState))]
[MemoryPack.MemoryPackUnion(1002, typeof(DroneInputsState))]
[MemoryPackable]
public partial interface I_InputState : IGONetEvent
{
    public PlayableEntityType EntityType { get; }
    public uint ClientTick { get; }
    public void SetClientTick(uint value);
}