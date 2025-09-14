using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject optionsPanel;   // Panel principal (sliders + botones)
    [SerializeField] private GameObject mainMenu;   // Panel principal (sliders + botones)

    [SerializeField] private GameObject creditsPanel;   // Panel de créditos

    [Header("Sliders")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider brightnessSlider;

    private OptionsData tempData; // Guardamos cambios temporales

    private void Start()
    {
        ResetPanels();

        // Cargar valores actuales desde OptionsManager
        if (OptionsManager.Instance != null)
        {
            tempData = new OptionsData
            {
                musicVolume = OptionsManager.Instance.Data.musicVolume,
                sfxVolume = OptionsManager.Instance.Data.sfxVolume,
                brightness = OptionsManager.Instance.Data.brightness
            };
        }
        else
        {
            tempData = new OptionsData();
            tempData.Load();
        }

        // Asignar valores a sliders
        musicSlider.value = tempData.musicVolume;
        sfxSlider.value = tempData.sfxVolume;
        brightnessSlider.value = tempData.brightness;

        // Listeners para actualizar valores temporales y aplicar en vivo
        musicSlider.onValueChanged.AddListener(OnMusicChanged);
        sfxSlider.onValueChanged.AddListener(OnSfxChanged);
        brightnessSlider.onValueChanged.AddListener(OnBrightnessChanged);
    }

    // ---- Gestión de paneles ----
    public void ResetPanels()
    {
        optionsPanel.SetActive(true);
        creditsPanel.SetActive(false);
    }

    public void OnSaveButton()
    {
        if (OptionsManager.Instance != null)
        {
            OptionsManager.Instance.Data.musicVolume = tempData.musicVolume;
            OptionsManager.Instance.Data.sfxVolume = tempData.sfxVolume;
            OptionsManager.Instance.Data.brightness = tempData.brightness;
            OptionsManager.Instance.Data.Save();
            OptionsManager.Instance.ApplyOptions(); // Aplica cambios globales
        }
    }

    public void OnCreditsButton()
    {
        creditsPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    public void OnCreditsBack()
    {
        creditsPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void OnExitButton()
    {
        mainMenu.GetComponent<MenuController>().CloseOptionsMenu();

        // Cierra menú opciones (se encarga MenuController de mostrar el menú principal)
        gameObject.SetActive(false);
    }

    // ---- Métodos para cambios en vivo ----
    public void OnMusicChanged(float value)
    {
        tempData.musicVolume = value;
        OptionsManager.Instance?.SetMusicVolume(value);
    }

    public void OnSfxChanged(float value)
    {
        tempData.sfxVolume = value;
        OptionsManager.Instance?.SetSfxVolume(value);
    }

    public void OnBrightnessChanged(float value)
    {
        tempData.brightness = value;
        OptionsManager.Instance?.SetBrightness(value);
    }
}
