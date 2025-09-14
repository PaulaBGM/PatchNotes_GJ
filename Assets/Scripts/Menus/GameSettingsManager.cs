using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;
using System.Collections;
using UnityEngine.UI;

public class GameSettingsManager : MonoBehaviour
{
    public static GameSettingsManager Instance { get; private set; }

    [Header("Audio")]
    [SerializeField] private AudioMixer _audioMixer;

    [Header("Brillo")]
    [SerializeField] private Image _brightnessOverlay;

    public float Brightness { get; private set; }
    public float MusicVolume { get; private set; }
    public float SFXVolume { get; private set; }

    private const string BrightnessKey = "Brightness";
    private const string MusicVolumeKey = "MusicVolume";
    private const string SFXVolumeKey = "SFXVolume";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        StartCoroutine(InitializeSettings());
    }

    private IEnumerator InitializeSettings()
    {
        // Carga del idioma
        yield return LocalizationSettings.InitializationOperation;

        Brightness = PlayerPrefs.GetFloat(BrightnessKey, 1.0f);
        MusicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 0.5f);
        SFXVolume = PlayerPrefs.GetFloat(SFXVolumeKey, 1.0f);

        ApplyBrightness(Brightness);
        ApplyVolume("MusicVolume", MusicVolume);
        ApplyVolume("SFXVolume", SFXVolume);

    }

    public void SetBrightness(float value)
    {
        Brightness = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat(BrightnessKey, Brightness);
        ApplyBrightness(Brightness);
    }

    public void SetMusicVolume(float value)
    {
        MusicVolume = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat(MusicVolumeKey, MusicVolume);
        ApplyVolume("MusicVolume", MusicVolume);
    }

    public void SetSFXVolume(float value)
    {
        SFXVolume = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat(SFXVolumeKey, SFXVolume);
        ApplyVolume("SFXVolume", SFXVolume);
    }

    private void ApplyBrightness(float value)
    {
        if (_brightnessOverlay != null)
        {
            float alpha = Mathf.Lerp(0.85f, 0f, value); // 0 = muy oscuro, 1 = sin oscurecer
            _brightnessOverlay.color = new Color(0, 0, 0, alpha);
        }
    }

    private void ApplyVolume(string parameter, float value)
    {
        _audioMixer.SetFloat(parameter, Mathf.Log10(Mathf.Max(value, 0.001f)) * 20f);
    }
}
