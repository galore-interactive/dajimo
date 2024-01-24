using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GONet;

public class Timer
{
    public static float GetServerTimeInClient()
    {
        return (float)GONetMain.Time.ElapsedSeconds - 0.02f - (GONetMain.GONetClient.connectionToServer.RTT_RecentAverage / 2f);
    }
}
