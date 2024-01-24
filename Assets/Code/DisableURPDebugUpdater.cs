using UnityEngine;
using UnityEngine.Rendering;

public class DisableURPDebugUpdater : MonoBehaviour
{
    private void Start()
    {
        //DebugManager.instance.enableRuntimeUI = false;
        Debug.LogError("Force the build console open...");
    }
}