using GONet;
using GONet.Utils;
using System;
using UnityEngine;

public class SerializationTesting : GONetParticipantCompanionBehaviour
{
    readonly Vector3Serializer v3serializer = new Vector3Serializer();

    protected override void Awake()
    {
        base.Awake();

        v3serializer.InitQuantizationSettings(18, -125, 125);
    }

    internal override void Tick(short uniqueTickHz, double elapsedSeconds, double deltaTime)
    {
        base.Tick(uniqueTickHz, elapsedSeconds, deltaTime);

        if (uniqueTickHz == 32)
        {
            if (IsMine)
            {
                BitByBitByteArrayBuilder bitStream_out, bitStream_in;
                SerializeDeserializeDiffCheck(out bitStream_out, out bitStream_in);
            }
            else
            {
                PreviousDiffCheck();
            }
        }
    }

    Vector3 position_prev;
    private void PreviousDiffCheck()
    {
        float diff = (transform.position - position_prev).magnitude;
        if (diff > 0.75f) GONetLog.Debug($"diff: {diff}");

        position_prev = transform.position;
    }

    private void SerializeDeserializeDiffCheck(out BitByBitByteArrayBuilder bitStream_out, out BitByBitByteArrayBuilder bitStream_in)
    {
        bitStream_out = BitByBitByteArrayBuilder.GetBuilder();
        v3serializer.Serialize(bitStream_out, gonetParticipant, transform.position);
        bitStream_out.WriteCurrentPartialByte();

        byte[] outd = new byte[bitStream_out.Length_WrittenBytes];
        Buffer.BlockCopy(bitStream_out.GetBuffer(), 0, outd, 0, outd.Length);
        bitStream_in = BitByBitByteArrayBuilder.GetBuilder_WithNewData(outd, outd.Length);
        Vector3 deserialized = v3serializer.Deserialize(bitStream_in).UnityEngine_Vector3;

        float diff = (transform.position - deserialized).magnitude;
        if (diff > 0.001f) GONetLog.Debug($"diff: {diff}");
    }
}