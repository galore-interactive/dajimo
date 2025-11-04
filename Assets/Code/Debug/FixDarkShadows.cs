using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Emergency fix for completely black shadows after Unity 6 upgrade.
/// This script forcibly sets ambient lighting at runtime to make shadows visible.
/// Attach this to any GameObject in your scene.
/// </summary>
public class FixDarkShadows : MonoBehaviour
{
    [Header("Ambient Lighting Settings")]
    [SerializeField] private Color ambientSkyColor = new Color(0.5f, 0.55f, 0.6f);
    [SerializeField] private Color ambientEquatorColor = new Color(0.35f, 0.38f, 0.42f);
    [SerializeField] private Color ambientGroundColor = new Color(0.25f, 0.23f, 0.2f);
    [SerializeField] private float ambientIntensity = 1.0f;

    [Header("Shadow Settings")]
    [SerializeField] private Color subtractiveShadowColor = new Color(0.42f, 0.48f, 0.63f);

    [Header("Debug")]
    [SerializeField] private bool showCurrentValues = true;

    private void Awake()
    {
        ApplyLightingFix();
    }

    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            ApplyLightingFix();
        }
    }

    public void ApplyLightingFix()
    {
        if (showCurrentValues)
        {
            Debug.Log($"[FixDarkShadows] BEFORE - Ambient Mode: {RenderSettings.ambientMode}");
            Debug.Log($"[FixDarkShadows] BEFORE - Sky Color: {RenderSettings.ambientSkyColor}");
            Debug.Log($"[FixDarkShadows] BEFORE - Equator Color: {RenderSettings.ambientEquatorColor}");
            Debug.Log($"[FixDarkShadows] BEFORE - Ground Color: {RenderSettings.ambientGroundColor}");
            Debug.Log($"[FixDarkShadows] BEFORE - Ambient Intensity: {RenderSettings.ambientIntensity}");
        }

        // Force Skybox ambient mode
        RenderSettings.ambientMode = AmbientMode.Trilight;

        // Set bright ambient colors
        RenderSettings.ambientSkyColor = ambientSkyColor;
        RenderSettings.ambientEquatorColor = ambientEquatorColor;
        RenderSettings.ambientGroundColor = ambientGroundColor;
        RenderSettings.ambientIntensity = ambientIntensity;

        // Fix shadow color
        RenderSettings.subtractiveShadowColor = subtractiveShadowColor;

        if (showCurrentValues)
        {
            Debug.Log($"[FixDarkShadows] AFTER - Ambient Mode: {RenderSettings.ambientMode}");
            Debug.Log($"[FixDarkShadows] AFTER - Sky Color: {RenderSettings.ambientSkyColor}");
            Debug.Log($"[FixDarkShadows] AFTER - Equator Color: {RenderSettings.ambientEquatorColor}");
            Debug.Log($"[FixDarkShadows] AFTER - Ground Color: {RenderSettings.ambientGroundColor}");
            Debug.Log($"[FixDarkShadows] AFTER - Ambient Intensity: {RenderSettings.ambientIntensity}");
        }

        Debug.Log("<color=green>[FixDarkShadows] Applied lighting fix! Shadows should now be visible.</color>");
    }
}
