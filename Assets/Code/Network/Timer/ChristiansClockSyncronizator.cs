using UnityEngine;

public class ChristiansClockSyncronizator : IGameTime
{
    private NetworkController _networkRPCController;
    private float _t0;
    private float _t1;
    private float _currentTime;

    public ChristiansClockSyncronizator(NetworkController networkRPCController)
    {
        _networkRPCController = networkRPCController;
        //_networkRPCController.OnServerTimeRecieved += ReceiveServerTime;

        AskForServerTime();
    }

    public void Update()
    {
        _currentTime += Time.deltaTime;
    }

    public float GetCurrentTime()
    {
        return _currentTime;
    }

    private void AskForServerTime()
    {
        _t0 = Time.time;
        //_networkRPCController.RequestServerTimeServerRpc();

        Debug.Log("sjdksd");
    }

    private void ReceiveServerTime(float serverTime)
    {
        _t1 = Time.time;
        Syncronize(serverTime);
    }

    private void Syncronize(float serverTime)
    {
        _currentTime = serverTime + ((_t1 - _t0) / 2f);
        Debug.Log($"Server time: {serverTime} seconds\nRequest's RTT: {_t1 - _t0} seconds\nSyncronized client time: {GetCurrentTime()} seconds");
    }
}
