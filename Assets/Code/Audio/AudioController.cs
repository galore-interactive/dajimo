using UnityEngine;

[DisallowMultipleComponent]
public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioStorageDataSO _audioStorageConfiguration;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _audioStorageConfiguration.Init();
    }

    public AudioClip GetAudioClipFromName(string name)
    {
        return _audioStorageConfiguration.GetAudioClipFromName(name);
    }
}
