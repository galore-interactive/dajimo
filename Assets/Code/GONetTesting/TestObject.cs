using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GONet;

public class TestObject : GONetParticipantCompanionBehaviour
{
    public override void OnGONetParticipantStarted()
    {
        base.OnGONetParticipantStarted();
        Debug.Log("YEAH");
    }
}
