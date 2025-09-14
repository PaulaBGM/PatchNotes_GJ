using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    public static OptionsManager Instance { get; private set; }
    public OptionsData Data { get; private set; } = new OptionsData();

    public delegate void BrightnessChanged(float value);
    public event BrightnessChanged OnBrightnessChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Cargar valores guardados
        Data.Load();

        // Aplicar al iniciar
        ApplyOptions();
    }

    public void ApplyOptions()
    {
        // Música
        if (MusicManager.Instance != null)
            MusicManager.Instance.SetGlobalVolume(Data.musicVolume);

        // Brillo
        OnBrightnessChanged?.Invoke(Data.brightness);

        // (Los efectos de sonido se aplican en cada AudioSource cuando se reproduzcan)
    }

    // ---- Métodos que usa OptionsMenuController ----
    public void SetMusicVolume(float value)
    {
        Data.musicVolume = value;
        if (MusicManager.Instance != null)
            MusicManager.Instance.SetGlobalVolume(value);
    }

    public void SetSfxVolume(float value)
    {
        Data.sfxVolume = value;
        // Aquí puedes propagar a un AudioManager si tienes uno
    }

    public void SetBrightness(float value)
    {
        Data.brightness = value;
        OnBrightnessChanged?.Invoke(value);
    }
}
