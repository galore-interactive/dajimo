using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GONet;
using GONet.PluginAPI;

public class GONetCustomEntityInterpolation
{
    public GONetSyncableValueTypes AppliesOnlyToGONetType => throw new System.NotImplementedException();

    public string Description => "This is a custom entity interpolation/extrapolation to smooth remote entities within a client.";

    //public bool TryGetBlendedValue(NumericValueChangeSnapshot[] valueBuffer, int valueCount, long atElapsedTicks, out GONetSyncableValue blendedValue)
    //{

    //    return true;
    //}
}
