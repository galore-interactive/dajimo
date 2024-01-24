using UnityEngine;
using UnityEngine.UI;

public class HitMarker : MonoBehaviour
{
    [SerializeField] private Image _hitMarkerImage;
    [SerializeField] private float _lifeTime = 0.2f;
    private float _lifeTimeLeft = 0f;
    private bool _isShowing = false;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _sound;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        SetVisibility(false);
    }

    public void ShowHitMarker()
    {
        _lifeTimeLeft = _lifeTime;
        PlayAudio();
        SetVisibility(true);
    }

    private void PlayAudio()
    {
        _audioSource.clip = _sound;
        _audioSource.Play();
    }

    private void SetVisibility(bool visibilityStatus)
    {
        _isShowing = visibilityStatus;
        _hitMarkerImage.enabled = visibilityStatus;
    }

    private void Update()
    {
        if(!_isShowing)
        {
            return;
        }

        _lifeTimeLeft -= Time.deltaTime;

        if(_lifeTimeLeft <= 0f)
        {
            _lifeTimeLeft = 0f;
            SetVisibility(false);
        }
    }
}
