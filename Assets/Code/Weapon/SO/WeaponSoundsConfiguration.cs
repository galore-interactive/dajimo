using UnityEngine;

[System.Serializable]
public class WeaponSoundsConfiguration
{
    [SerializeField] private string _shotAudioName;
    [SerializeField] private string _holsterAudioName;
    [SerializeField] private string _hideAudioName;
    [SerializeField] private string _reloadAudioName;

    public string ShotAudioName => _shotAudioName;
    public string HolsterAudioName => _holsterAudioName;
    public string HideAudioName => _hideAudioName;
    public string ReloadAudioName => _reloadAudioName;

    public WeaponSoundsConfiguration() { }

    public WeaponSoundsConfiguration(string shotAudioName, string holsterAudioName, string hideAudioName, string reloadAudioName)
    {
        _shotAudioName = shotAudioName;
        _holsterAudioName = holsterAudioName;
        _hideAudioName = hideAudioName;
        _reloadAudioName = reloadAudioName;
    }
}
