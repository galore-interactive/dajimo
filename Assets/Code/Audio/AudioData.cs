using System;
using UnityEngine;

[Serializable]
public class AudioData
{
    [SerializeField] private string _name;
    [SerializeField] private AudioClip _audio;

    public string Name => _name;
    public AudioClip Audio => _audio;
}
