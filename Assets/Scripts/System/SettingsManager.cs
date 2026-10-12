using System;
using UnityEngine;
using UnityEngine.UI;
public class SettingsManager : MonoBehaviour
{

    public static SettingsManager Instance;

    // General

    [SerializeField] private Toggle powerSavingToggle;
    public bool PowerSavingEnabled;

    // Accessibility

    [SerializeField] private Toggle limitedMotionToggle;
    public bool LimitedMotionEnabled;

    public static event Action<bool> OnSimpleFontToggleChanged;
    [SerializeField] private Toggle simpleFontToggle;
    public bool SimpleFontEnabled;

    [SerializeField] private Slider textScaleSlider;
    public float TextScale;

    // Audio & Haptics

    [SerializeField] private Slider musicVolumeSlider;
    public float MusicMutliplier;

    [SerializeField] private Slider sfxVolumeSlider;
    public float SFXMultiplier;

    [SerializeField] private Toggle vibrationsEnabledToggle;
    public bool VibrationsEnabled;
    private void Awake()
    {
        Instance = this;
    }
    // General
    public void UpdatePowerSaving()
    {
        PowerSavingEnabled = powerSavingToggle.isOn;
    }
    // Accessibility
    public void UpdateLimitedMotion()
    {
        LimitedMotionEnabled = limitedMotionToggle.isOn;
    }

    public void UpdateSimpleFont()
    {
        SimpleFontEnabled = simpleFontToggle.isOn;
        OnSimpleFontToggleChanged?.Invoke(SimpleFontEnabled);
    }
    public void UpdateTextScale()
    {
        TextScale = textScaleSlider.value;
    }

    // Audio & Haptics
    public void UpdateMusicMultiplier()
    {
        MusicMutliplier = musicVolumeSlider.value;
    }
    public void UpdateSFXMultiplier()
    {
        SFXMultiplier = sfxVolumeSlider.value;
    }
    public void UpdateVibrationsEnabled()
    {
        VibrationsEnabled = vibrationsEnabledToggle.isOn;
    }
}
