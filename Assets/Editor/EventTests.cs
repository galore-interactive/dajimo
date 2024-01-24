using GONet;
using GONet.Utils;
using NUnit.Framework;
using UnityEngine;

namespace Assets
{
    [TestFixture]
    public class EventTests
    {
        [Test]
        public void Test()
        {
            LocalContextEntityEvent originalEvent = new LocalContextEntityEvent();
            originalEvent.GONetId = 123;

            byte[] serializedBytes = 
                SerializationUtils.SerializeToBytes(originalEvent, out int bytesUsed, out bool doesNeedReturnPool);
            Debug.Log($"raw/UNcompressed bytes#: {bytesUsed}");

            GONetMain.AutoCompressEverything.Compress(serializedBytes, (ushort)bytesUsed, out byte[] messageBytesCompressed, out ushort messageBytesCompressedUsedCount);
            Debug.Log($"compressed bytes#: {messageBytesCompressedUsedCount}");

            LocalContextEntityEvent deserializedEvent = 
                SerializationUtils.DeserializeFromBytes<LocalContextEntityEvent>(serializedBytes);

            Assert.AreEqual(originalEvent.GONetId, deserializedEvent.GONetId);
            Assert.AreEqual(Vector3.zero, deserializedEvent.vector3);
            Assert.AreEqual(Quaternion.identity, deserializedEvent.quaternion);

            //////////////////////////////////////////////////////////////////////////////////////////////////
            originalEvent = new LocalContextEntityEvent();
            originalEvent.vector3 = new Vector3(11, 22, 33);
            originalEvent.quaternion = Quaternion.Euler(new Vector3(11, 22, 33));

            serializedBytes =
                SerializationUtils.SerializeToBytes(originalEvent, out bytesUsed, out doesNeedReturnPool);
            Debug.Log($"raw/UNcompressed bytes#: {bytesUsed}");

            GONetMain.AutoCompressEverything.Compress(serializedBytes, (ushort)bytesUsed, out messageBytesCompressed, out messageBytesCompressedUsedCount);
            Debug.Log($"compressed bytes#: {messageBytesCompressedUsedCount}");

            deserializedEvent =
                SerializationUtils.DeserializeFromBytes<LocalContextEntityEvent>(serializedBytes);

            Assert.AreEqual(0, deserializedEvent.GONetId);
            Assert.AreEqual(originalEvent.vector3, deserializedEvent.vector3);
            Assert.AreEqual(originalEvent.quaternion, deserializedEvent.quaternion);
        }
    }
}
