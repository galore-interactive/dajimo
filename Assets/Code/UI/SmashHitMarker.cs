using UnityEngine;
using UnityEngine.UI;

public class SmashHitMarker : MonoBehaviour
{
    [SerializeField] private Image _hitMarkerImage;
    [SerializeField] private float _lifeTime = 0.2f;
    private float _lifeTimeLeft = 0f;
    private bool _isShowing = false;

    private void Start()
    {
        SetVisibility(false);
    }

    public void ShowHitMarker()
    {
        _lifeTimeLeft = _lifeTime;
        SetVisibility(true);
    }

    private void SetVisibility(bool visibilityStatus)
    {
        _isShowing = visibilityStatus;
        _hitMarkerImage.enabled = visibilityStatus;
    }

    private void Update()
    {
        if (!_isShowing)
        {
            return;
        }

        _lifeTimeLeft -= Time.deltaTime;

        if (_lifeTimeLeft <= 0f)
        {
            _lifeTimeLeft = 0f;
            SetVisibility(false);
        }
    }
}
