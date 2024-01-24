using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class EmptyInterpolationBufferDisplay : MonoBehaviour
{
    private TextMeshProUGUI _textComponent;
    private uint _currentEmptyInterpolationBuffer = 0;

    private void Awake()
    {
        _textComponent = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        UpdateText();
    }

    private void OnEnable()
    {
        ProjectiveVelocityBlendingEntityInterpolation.OnEmptyBuffer += IncreaseDisplay;
    }

    private void OnDisable()
    {
        ProjectiveVelocityBlendingEntityInterpolation.OnEmptyBuffer -= IncreaseDisplay;
    }

    private void IncreaseDisplay()
    {
        _currentEmptyInterpolationBuffer++;
        UpdateText();
    }

    private void UpdateText()
    {
        _textComponent.text = $"Empty Interpolation Buffer: {_currentEmptyInterpolationBuffer}";
    }
}
