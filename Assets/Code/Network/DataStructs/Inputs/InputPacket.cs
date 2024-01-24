using MemoryPack;
using GONet;

[MemoryPackable]
public partial class InputPacket : ITransientEvent
{
    public I_InputState inputState0;
    public I_InputState inputState1;

    [MemoryPackIgnore]
    public long OccurredAtElapsedTicks => 0;

    /// <summary>
    /// We only want this message to go from a client to the server.
    /// We do NOT want the default GONet behavior where the server will take what it receives from a client and send it to all other clients.
    /// </summary>
    [MemoryPackIgnore]
    public bool IsSingularRecipientOnly => true;


    public InputPacket(I_InputState inputState0, I_InputState inputState1 = default)
    {
        this.inputState0 = inputState0;
        this.inputState1 = inputState1;
    }
}
