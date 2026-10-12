using System;
using UnityEngine;
using UnityEngine.UI;
public class SettingsManager : MonoBehaviour
{

    public static SettingsManager Instance;

    // General

    [SerializeField] private Toggle powerSavingToggle;
    public bool PowerSavingEnabled = false;

    // Accessibility

    [SerializeField] private Toggle limitedMotionToggle;
    public bool LimitedMotionEnabled = false;
;
    [SerializeField] private Toggle simpleFontToggle;
    public bool SimpleFontEnabled = false;

    [SerializeField] private Slider textScaleSlider;
    public float TextScale = 1;

    // Audio & Haptics

    [SerializeField] private Slider musicVolumeSlider;
    public float MusicMutliplier = 1;

    [SerializeField] private Slider sfxVolumeSlider;
    public float SFXMultiplier = 1;

    [SerializeField] private Toggle vibrationsEnabledToggle;
    public bool VibrationsEnabled = false;
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
