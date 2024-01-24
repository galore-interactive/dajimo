using UnityEngine;

public class LimitFramerate : MonoBehaviour
{
    private void Awake()
    {
#if UNITY_EDITOR
     QualitySettings.vSyncCount = 0;  // VSync must be disabled
     Application.targetFrameRate = 45;
#endif
        Application.targetFrameRate = 60;
    }
}
