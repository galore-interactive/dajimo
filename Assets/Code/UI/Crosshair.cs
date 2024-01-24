using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private RectTransform _crosshairRectTransform;
    private Image[] _crosshairImages;
    [SerializeField] private RectTransform middleDot;

    private float _idleSize = 50f;

    private float _currentSize;
    private float _targetSize;
    private float _sizeSpeed = 6.5f;

    private float _visibilitySpeed = 9f;
    private float _currentAlpha = 1f;
    private float _targetAlpha = 1f;

    private void Awake()
    {
        _targetSize = _idleSize;
        _currentSize = _idleSize;
        _crosshairImages = GetComponentsInChildren<Image>();
    }

    private void Update()
    {
        UpdateSize();
        ApplySize();
        UpdateVisibility();
        ApplyVisibility();
    }

    private void UpdateSize()
    {
        _currentSize = Mathf.Lerp(_currentSize, _targetSize, Time.deltaTime * _sizeSpeed);
    }

    private void ApplySize()
    {
        _crosshairRectTransform.sizeDelta = new Vector2(_currentSize, _currentSize);
    }

    private void UpdateVisibility()
    {
        _currentAlpha = Mathf.Lerp(_currentAlpha, _targetAlpha, Time.deltaTime * _visibilitySpeed);
    }

    private void ApplyVisibility()
    {
        for (int i = 0; i < _crosshairImages.Length; i++)
        {
            _crosshairImages[i].color = new Color(_crosshairImages[i].color.r, _crosshairImages[i].color.g, _crosshairImages[i].color.b, _currentAlpha);
        }
    }

    public void SetSize(float size)
    {
        _targetSize = size;
    }

    public void SetCenterDotVisibility(bool shouldShow)
    {
        middleDot?.gameObject.SetActive(shouldShow);
    }

    public void Hide()
    {
        _targetAlpha = 0f;
    }

    public void Show()
    {
        _targetAlpha = 1f;
    }
}
