/*using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using GONet.Utils;
using MemoryPack;
using System;
using System.IO;

public class Custom_FPS_Structures
{
    
    #region Serialization And Deserialization with GONet Serialization Utils
    [Test]
    public void SerializingAndDeserializingEntityState_WithGONetSerializationUtils_DeserializatedStructShouldBeEqualToInitialStruct()
    {
        //Arrange
        EntityState initialEntityState = new EntityState(0, new Vector2(5, 9), new Vector3(3, 3, 3), new Vector3(99, 22, 44), new Vector3(1, 3, 5), true, false);

        //Act
        byte[] resultingBytes = SerializeWithGONetSerializationUtils(initialEntityState);
        EntityState deserializedEntityState = SerializationUtils.DeserializeFromBytes<EntityState>(resultingBytes);

        //Assert
        Assert.IsTrue(deserializedEntityState.cameraLookAtEulerAngles.x == initialEntityState.cameraLookAtEulerAngles.x);
    }

    [Test]
    public void SerializingAndDeserializingPlayerState_WithGONetSerializationUtils_DeserializatedStructShouldBeEqualToInitialStruct()
    {
        //Arrange
        PlayerState initialPlayerState = new PlayerState();
        initialPlayerState.clientTick = 9876;
        initialPlayerState.characterControllerHeight = 33;

        //Act
        byte[] resultingBytes = SerializeWithGONetSerializationUtils(initialPlayerState);
        PlayerState deserializedPlayerState = DeserializeWithGONetSerializationUtils<PlayerState>(resultingBytes);

        //Assert
        Assert.IsTrue(deserializedPlayerState.clientTick == initialPlayerState.clientTick && deserializedPlayerState.characterControllerHeight == initialPlayerState.characterControllerHeight);
    }

    [Test]
    public void SerializingAndDeserializingSnapshotState_WithGONetSerializationUtils_DeserializatedStructShouldBeEqualToInitialStruct()
    {
        //Arrange
        SnapshotState initialSnapshotState = new SnapshotState(false, new PlayerState(), 44.0f, 455, 0);

        //Act
        byte[] resultingBytes = SerializeWithGONetSerializationUtils(initialSnapshotState);
        SnapshotState deserializedSnapshotState = DeserializeWithGONetSerializationUtils<SnapshotState>(resultingBytes);

        //Assert
        Assert.IsTrue(deserializedSnapshotState.hasPlayerState == initialSnapshotState.hasPlayerState && deserializedSnapshotState.serverTick == initialSnapshotState.serverTick);
    }

    [Test]
    public void SerializingAndDeserializingEntityEvent_WithGONetSerializationUtils_DeserializatedStructShouldBeEqualToInitialStruct()
    {
        //Arrange
        EntityEvent initialEntityEvent = new EntityEvent(EntityEventType.EV_FIRE_WEAPON, true, new byte[3], 1);

        //Act
        byte[] resultingBytes = SerializeWithGONetSerializationUtils(initialEntityEvent);
        EntityEvent deserializedEntityEvent = DeserializeWithGONetSerializationUtils<EntityEvent>(resultingBytes);

        //Assert
        Assert.IsTrue(deserializedEntityEvent.type == initialEntityEvent.type && deserializedEntityEvent.parameters.Length == initialEntityEvent.parameters.Length && deserializedEntityEvent.GONetId == initialEntityEvent.GONetId);
    }

    [Test]
    public void SerializingAndDeserializingInputState_WithGONetSerializationUtils_DeserializatedStructShouldBeEqualToInitialStruct()
    {
        //Arrange
        InputState initialInputState = new InputState(new Vector2(-10, 30), new Vector2(20, 1), false, false, true, false, false, 33);

        //Act
        byte[] resultingBytes = SerializeWithGONetSerializationUtils(initialInputState);
        InputState deserializedInputState = DeserializeWithGONetSerializationUtils<InputState>(resultingBytes);

        //Assert
        Assert.IsTrue(deserializedInputState.clientTick == initialInputState.clientTick && deserializedInputState.movementInput.x == initialInputState.movementInput.x);
    }

    [Test]
    public void SerializingAndDeserializingInputPacket_WithGONetSerializationUtils_DeserializatedStructShouldBeEqualToInitialStruct()
    {
        //Arrange
        InputState[] inputStatesArray = new InputState[2];

        InputState inputState1 = new InputState(new Vector2(-10, 30), new Vector2(20, 1), false, false, true, false, false, 33);
        InputState inputState2 = new InputState(new Vector2(50, -2), new Vector2(-20, -1), false, true, false, true, true, 34);

        inputStatesArray[0] = inputState1;
        inputStatesArray[1] = inputState2;

        InputPacket initialInputPacket = new InputPacket(inputStatesArray);

        //Act
        byte[] resultingBytes = SerializeWithGONetSerializationUtils(initialInputPacket);
        InputPacket deserializedInputPacket = DeserializeWithGONetSerializationUtils<InputPacket>(resultingBytes);

        //Assert
        Assert.IsTrue(deserializedInputPacket.inputStates.Length == initialInputPacket.inputStates.Length && deserializedInputPacket.inputStates[0].movementInput.x == initialInputPacket.inputStates[0].movementInput.x);
    }

    private byte[] SerializeWithGONetSerializationUtils<T>(T structureToSerialize)
    {
        return SerializationUtils.SerializeToBytes(structureToSerialize, out int numberOfBytes);
    }

    private T DeserializeWithGONetSerializationUtils<T>(byte[] bytesToDeserialize)
    {
        return MessagePackSerializer.Deserialize<T>(bytesToDeserialize);
    }
    #endregion

    #region Serialization And Deserialization with Message Pack
    [Test]
    public void SerializingAndDeserializingPlayerState_WithMessagePack_DeserializatedStructShouldBeEqualToInitialStruct()
    {
        //Arrange
        PlayerState initialPlayerState = new PlayerState();
        initialPlayerState.clientTick = 9876;
        initialPlayerState.characterControllerHeight = 33;

        //Act
        Stream memoryStream = SerializeWithMessagePackThroughStream(initialPlayerState);
        PlayerState deserializedPlayerState = DeserializeWithMessagePack<PlayerState>(memoryStream);

        //Assert
        Assert.IsTrue(deserializedPlayerState.clientTick == initialPlayerState.clientTick && deserializedPlayerState.characterControllerHeight == initialPlayerState.characterControllerHeight);
    }

    [Test]
    public void SerializingAndDeserializingSnapshotState_WithMessagePack_DeserializatedStructShouldBeEqualToInitialStruct()
    {
        //Arrange
        SnapshotState initialSnapshotState = new SnapshotState(false, new PlayerState(), 44.0f, 455, 0);

        //Act
        Stream memoryStream = SerializeWithMessagePackThroughStream(initialSnapshotState);
        SnapshotState deserializedSnapshotState = DeserializeWithMessagePack<SnapshotState>(memoryStream);

        //Assert
        Assert.IsTrue(deserializedSnapshotState.hasPlayerState == initialSnapshotState.hasPlayerState && deserializedSnapshotState.serverTick == initialSnapshotState.serverTick);
    }

    [Test]
    public void SerializingAndDeserializingEntityState_WithMessagePack_DeserializatedStructShouldBeEqualToInitialStruct()
    {
        //Arrange
        EntityState initialEntityState = new EntityState(0, new Vector2(5, 9), new Vector3(3, 3, 3), new Vector3(99, 22, 44), new Vector3(1, 3, 5), true, false);

        //Act
        Stream memoryStream = SerializeWithMessagePackThroughStream(initialEntityState);
        EntityState deserializedEntityState = DeserializeWithMessagePack<EntityState>(memoryStream);

        //Assert
        Assert.IsTrue(deserializedEntityState.cameraLookAtEulerAngles.x == initialEntityState.cameraLookAtEulerAngles.x);
    }

    [Test]
    public void SerializingAndDeserializingEntityEvent_WithMessagePack_DeserializatedStructShouldBeEqualToInitialStruct()
    {
        //Arrange
        EntityEvent initialEntityEvent = new EntityEvent(EntityEventType.EV_FIRE_WEAPON, true, new byte[3], 1);

        //Act
        Stream memoryStream = SerializeWithMessagePackThroughStream(initialEntityEvent);
        EntityEvent deserializedEntityEvent = DeserializeWithMessagePack<EntityEvent>(memoryStream);

        //Assert
        Assert.IsTrue(deserializedEntityEvent.type == initialEntityEvent.type && deserializedEntityEvent.parameters.Length == initialEntityEvent.parameters.Length && deserializedEntityEvent.GONetId == initialEntityEvent.GONetId);
    }

    [Test]
    public void SerializingAndDeserializingInputState_WithMessagePack_DeserializatedStructShouldBeEqualToInitialStruct()
    {
        //Arrange
        InputState initialInputState = new InputState(new Vector2(-10, 30), new Vector2(20, 1), false, false, true, false, false, 33);

        //Act
        Stream memoryStream = SerializeWithMessagePackThroughStream(initialInputState);
        InputState deserializedInputState = DeserializeWithMessagePack<InputState>(memoryStream);

        //Assert
        Assert.IsTrue(deserializedInputState.clientTick == initialInputState.clientTick && deserializedInputState.movementInput.x == initialInputState.movementInput.x);
    }

    [Test]
    public void SerializingAndDeserializingInputPacket_WithMessagePack_DeserializatedStructShouldBeEqualToInitialStruct()
    {
        //Arrange
        InputState[] inputStatesArray = new InputState[2];

        InputState inputState1 = new InputState(new Vector2(-10, 30), new Vector2(20, 1), false, false, true, false, false, 33);
        InputState inputState2 = new InputState(new Vector2(50, -2), new Vector2(-20, -1), false, true, false, true, true, 34);

        inputStatesArray[0] = inputState1;
        inputStatesArray[1] = inputState2;

        InputPacket initialInputPacket = new InputPacket(inputStatesArray);

        //Act
        Stream memoryStream = SerializeWithMessagePackThroughStream(initialInputPacket);
        InputPacket deserializedInputPacket = DeserializeWithMessagePack<InputPacket>(memoryStream);

        //Assert
        Assert.IsTrue(deserializedInputPacket.inputStates.Length == initialInputPacket.inputStates.Length && deserializedInputPacket.inputStates[0].movementInput.x == initialInputPacket.inputStates[0].movementInput.x);
    }

    private Stream SerializeWithMessagePackThroughStream<T>(T structureToSerialize)
    {
        Stream memoryStream = new MemoryStream();
        MessagePackSerializer.Serialize(memoryStream, structureToSerialize);

        return memoryStream;
    }

    private T DeserializeWithMessagePack<T>(Stream memoryStreamToDeserialize)
    {
        return MessagePackSerializer.Deserialize<T>(memoryStreamToDeserialize);
    }
    #endregion
}
    */