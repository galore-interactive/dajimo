using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    private AudioSource _audioSource;
    private AudioController _audioController;

    private void Awake()
    {
        _audioController = FindObjectOfType<AudioController>();

        _audioSource = GetComponent<AudioSource>();
        ConfigureAudioSource();
    }

    private void ConfigureAudioSource()
    {
        _audioSource.playOnAwake = false;
    }

    public void PlayAudio(string audioName)
    {
        _audioSource.clip = _audioController.GetAudioClipFromName(audioName);
        _audioSource.Play();
    }
}
