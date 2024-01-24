using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu(menuName = "Scriptable Objects/Audio/Audio Storage", fileName = "AudioStorageData")]
public class AudioStorageDataSO : ScriptableObject
{
    [SerializeField] private List<AudioData> _audios;
    private Dictionary<string, AudioClip> _audioStorage;
    private bool _isInitialized = false;

    public void Init()
    {
        _audioStorage = GetAudioDataDictionary();
        _isInitialized = true;
    }

    private Dictionary<string, AudioClip> GetAudioDataDictionary()
    {
        Dictionary<string, AudioClip> audioDictionary = new Dictionary<string, AudioClip>();
        bool status = true;

        foreach(AudioData audio in _audios)
        {
            status = audioDictionary.TryAdd(audio.Name, audio.Audio);

#if UNITY_EDITOR
            if(!status)
            {
                Debug.LogWarning($"The are more than one audio called {audio.Name}.");
            }
#endif
        }

        return audioDictionary;
    }

    public AudioClip GetAudioClipFromName(string name)
    {
        Assert.IsTrue(_isInitialized, $"[AudioStorageDataSO at GetAudioClipFromName]: The audioStorage needs to be initialized. Call Init() method");

        AudioClip wantedAudioClip;
        bool status = _audioStorage.TryGetValue(name, out wantedAudioClip);

        Assert.IsTrue(status, $"[AudioStorageDataSO at GetAudioClipFromName]: There is not an audio called {name} in the storage");

        return wantedAudioClip;
    }
}
