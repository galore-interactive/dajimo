using GONet;
using MemoryPack;
using UnityEngine;

[MemoryPackable]
public partial class LocalContextEntityEvent : ITransientEvent, IHaveRelatedGONetId
{
    public EntityEventType type;
    public bool hasParameters;
    public byte[] parameters;
    public uint GONetParticipantId;

    #region optional values that are implemented specially here to only have a minimal size impact when not present (i.e., one bool, or a byte if alignment requires it)

    [MemoryPackIgnore] public Vector3 vector3 = Vector3.zero;
    [MemoryPackIgnore] public Quaternion quaternion = Quaternion.identity;

    [MemoryPackOnSerialized]
    static void OnSerialized_WriteOptional(ref MemoryPackWriter writer, ref LocalContextEntityEvent? value)
    {
        bool hasOptional = value.vector3 != Vector3.zero || value.quaternion != Quaternion.identity;
        writer.WriteValue(hasOptional);

        if (hasOptional)
        {
            writer.WriteValue(value.vector3);
            writer.WriteValue(value.quaternion);
        }
    }

    [MemoryPackOnDeserialized]
    static void OnDeserialized_ReadOptional(ref MemoryPackReader reader, ref LocalContextEntityEvent? value)
    {
        bool hasOptional = default;
        reader.ReadValue(ref hasOptional);
        
        if (hasOptional)
        {
            reader.ReadValue(ref value.vector3);
            reader.ReadValue(ref value.quaternion);
        }
    }

    #endregion

    [MemoryPackIgnore]
    public uint GONetId { get => GONetParticipantId; set => GONetParticipantId = value; }
    [MemoryPackIgnore]
    public long OccurredAtElapsedTicks => 0;

    public LocalContextEntityEvent() { }

    public LocalContextEntityEvent(EntityEventType type, uint GONetParticipantId) : this(type, hasParameters: false, default, GONetParticipantId)
    {
    }

    [MemoryPackConstructor]
    public LocalContextEntityEvent(EntityEventType type, bool hasParameters, byte[] parameters, uint GONetParticipantId)
    {
        this.type = type;
        this.hasParameters = hasParameters;
        this.parameters = parameters;
        this.GONetParticipantId = GONetParticipantId;
    }

}
