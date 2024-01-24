using System.Collections.Generic;
using UnityEngine;

public class ServerInputsPlayoutDelayBufferController
{
    private class ClientBufferInfo
    {
        private List<ClientInputStateInformation> _inputsBuffer;
        public I_InputState lastInputState;
        public uint emptyBufferTimes;
        public uint lastInputTicksInBuffer;
        public uint LastInputTick => lastInputState.ClientTick;

        public ClientBufferInfo()
        {
            _inputsBuffer = new List<ClientInputStateInformation>();
        }

        public void AddInputState(I_InputState newInputState)
        {
            //Debug.Log($"Adding InputState...");

            _inputsBuffer.Add(new ClientInputStateInformation(newInputState));
        }

        public I_InputState GetNextInputState()
        {
            ClientInputStateInformation currentInfo = _inputsBuffer[0];
            _inputsBuffer.RemoveAt(0);
            lastInputTicksInBuffer = currentInfo.ticksInBuffer;
            return currentInfo.inputState;
        }

        public int GetNumberOfInputStatesInBuffer()
        {
            return _inputsBuffer.Count;
        }

        public void UpdateBuffer()
        {
            if(_inputsBuffer.Count == 0)
            {
                lastInputTicksInBuffer = 0;
                return;
            }

            foreach(ClientInputStateInformation inputInfo in _inputsBuffer)
            {
                ++inputInfo.ticksInBuffer;
            }
        }
    }

    private class ClientInputStateInformation
    {
        public I_InputState inputState;
        public uint ticksInBuffer;

        public ClientInputStateInformation(I_InputState inputState)
        {
            this.inputState = inputState;
            ticksInBuffer = 1;
        }
    }

    private Dictionary<ushort, ClientBufferInfo> _playoutDelayBuffer;
    private const uint MAXIMUM_PLAYOUT_BUFFER_SIZE = 2;
    private const uint EMPTY_BUFFER_PREDICTION_THRESHOLD = 2;

    public ServerInputsPlayoutDelayBufferController()
    {
        _playoutDelayBuffer = new Dictionary<ushort, ClientBufferInfo>();
    }

    /// <summary>
    /// Stores a client's InputPacket into the Input Playout delay buffer.
    /// </summary>
    /// <param name="inputPacket">The InputPacket to store</param>
    /// <param name="clientAuthorityId">The client ID of the InputPacket to store</param>
    public void ReceiveClientInputState(InputPacket inputPacket, ushort clientAuthorityId)
    {
        ReceiveClientInputState(inputPacket.inputState0, clientAuthorityId);
        ReceiveClientInputState(inputPacket.inputState1, clientAuthorityId);
    }

    private void ReceiveClientInputState(I_InputState inputState, ushort clientAuthorityId)
    {
        //Debug.Log($"Receive client input state.  isDefault? {inputState == default}");

        if (inputState == default) return;

        if (_playoutDelayBuffer.ContainsKey(clientAuthorityId))
        {
            if (IsInputStateValid(inputState, clientAuthorityId))
            {
                //Debug.Log("VALID INPUT STATE");
                AddInputStateToBuffer(inputState, clientAuthorityId);
            }
            else
            {
                //Debug.Log("NOT VALID INPUT STATE");
            }
        }
        else
        {
            CreateNewClientInfo(clientAuthorityId);
            AddInputStateToBuffer(inputState, clientAuthorityId);
        }
    }

    private bool IsInputStateValid(I_InputState inputState, ushort clientAuthorityId)
    {
        return inputState.ClientTick > _playoutDelayBuffer[clientAuthorityId].LastInputTick;
    }

    private void AddInputStateToBuffer(I_InputState inputState, ushort clientAuthorityId)
    {
        _playoutDelayBuffer[clientAuthorityId].AddInputState(inputState);
        _playoutDelayBuffer[clientAuthorityId].lastInputState = inputState;
    }

    private void CreateNewClientInfo(ushort clientAuthorityId)
    {
        ClientBufferInfo bufferInfo = new ClientBufferInfo();
        _playoutDelayBuffer.Add(clientAuthorityId, bufferInfo);
    }

    /// <summary>
    /// Returns a collection of InputState per client to be simulated in the current tick based on the Input playout delay buffer.
    /// </summary>
    /// <returns></returns>
    public Dictionary<ushort, Queue<I_InputState>> GetInputsToSimulate()
    {
        Dictionary<ushort, Queue<I_InputState>> clientInputsToSimulate = new Dictionary<ushort, Queue<I_InputState>>();
        foreach (KeyValuePair<ushort, ClientBufferInfo> playoutBufferClient in _playoutDelayBuffer)
        {
            if (playoutBufferClient.Value.GetNumberOfInputStatesInBuffer() > 0)
            {
                playoutBufferClient.Value.emptyBufferTimes = 0;
                Queue<I_InputState> clientInputs = GetInputsToSimulateFromClientBuffer(playoutBufferClient.Value);

                clientInputsToSimulate.Add(playoutBufferClient.Key, clientInputs);
            }
            else
            {
                playoutBufferClient.Value.emptyBufferTimes++;

                if(playoutBufferClient.Value.emptyBufferTimes >= EMPTY_BUFFER_PREDICTION_THRESHOLD)
                {
                    I_InputState predictedInputState = PredictInputState(playoutBufferClient.Value);
                    Queue<I_InputState> clientInputs = new Queue<I_InputState>();
                    clientInputs.Enqueue(predictedInputState);
                    clientInputsToSimulate.Add(playoutBufferClient.Key, clientInputs);

                    playoutBufferClient.Value.lastInputState = predictedInputState;
                }
            }
        }

        return clientInputsToSimulate;
    }

    private Queue<I_InputState> GetInputsToSimulateFromClientBuffer(ClientBufferInfo clientBufferInfo)
    {
        Queue<I_InputState> clientInputs = new Queue<I_InputState>();

        clientInputs.Enqueue(clientBufferInfo.GetNextInputState());

        while (clientBufferInfo.GetNumberOfInputStatesInBuffer() > MAXIMUM_PLAYOUT_BUFFER_SIZE)
        {
            clientInputs.Enqueue(clientBufferInfo.GetNextInputState());
        }

        return clientInputs;
    }

    private I_InputState PredictInputState(ClientBufferInfo clientBufferInfo)
    {
        //Debug.LogWarning("SERVER PREDICT InputState!!");
        I_InputState predictedInput = clientBufferInfo.lastInputState;
        if(clientBufferInfo.emptyBufferTimes == EMPTY_BUFFER_PREDICTION_THRESHOLD)
        {
            predictedInput.SetClientTick(predictedInput.ClientTick + EMPTY_BUFFER_PREDICTION_THRESHOLD);
        }
        else
        {
            predictedInput.SetClientTick(predictedInput.ClientTick + 1);
        }

        return predictedInput;
    }

    /// <summary>
    /// Removes a client from the Input playout delay buffer.
    /// </summary>
    /// <param name="clientAuthorityId">The Authority ID of the client to remove</param>
    public void RemoveClientFromBuffer(ushort clientAuthorityId)
    {
        if(_playoutDelayBuffer.ContainsKey(clientAuthorityId))
        {
            _playoutDelayBuffer.Remove(clientAuthorityId);
        }
        else
        {
            Debug.LogWarning("The client does not exist in the playout buffer");
        }
    }

    public void UpdateBuffers()
    {
        foreach(ClientBufferInfo buffer in _playoutDelayBuffer.Values)
        {
            buffer.UpdateBuffer();
        }
    }

    public IReadOnlyDictionary<ushort, uint> GetCurrentTicksInBufferPerClient()
    {
        Dictionary<ushort, uint> returnDictionary = new Dictionary<ushort, uint>();

        foreach(KeyValuePair<ushort, ClientBufferInfo> keyValuePair in _playoutDelayBuffer)
        {
            returnDictionary.Add(keyValuePair.Key, keyValuePair.Value.lastInputTicksInBuffer);
        }

        return returnDictionary;
    }
}
