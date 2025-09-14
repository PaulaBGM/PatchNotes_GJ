using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsReferenceManager : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Scrollbar _brightSlider;
    [SerializeField] private Scrollbar _musicVolumeSlider;
    [SerializeField] private Scrollbar _sfxVolumeSlider;

    [Header("UI Panels")]
    [SerializeField] private GameObject _optionsMenuExit;
    [SerializeField] private GameObject _creditsMenu;
    [SerializeField] private GameObject _optionMenu;

    private bool hasChanges;

    private void Start()
    {
        if (GameSettingsManager.Instance == null)
        {
            Debug.LogWarning("GameSettingsManager no está presente en la escena.");
            return;
        }

        // Cargar valores desde el manager
        _brightSlider.value = GameSettingsManager.Instance.Brightness;
        _musicVolumeSlider.value = GameSettingsManager.Instance.MusicVolume;
        _sfxVolumeSlider.value = GameSettingsManager.Instance.SFXVolume;

        // Escuchar cambios
        _brightSlider.onValueChanged.AddListener(OnBrightnessChanged);
        _musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        _sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);

        _creditsMenu.SetActive(false);
        _optionsMenuExit.SetActive(false);
    }

    private void OnBrightnessChanged(float value)
    {
        GameSettingsManager.Instance.SetBrightness(value);
    }

    private void OnMusicVolumeChanged(float value)
    {
        GameSettingsManager.Instance.SetMusicVolume(value);
    }

    private void OnSFXVolumeChanged(float value)
    {
        GameSettingsManager.Instance.SetSFXVolume(value);
    }

    public void ExitCreditsMenu()
    {
        _creditsMenu.SetActive(false);
        _optionMenu.SetActive(true);
    }

    public void CheckChanges()
    {
        var gsm = GameSettingsManager.Instance;
        if (gsm == null) return;

        hasChanges =
            _brightSlider.value != gsm.Brightness ||
            _musicVolumeSlider.value != gsm.MusicVolume ||
            _sfxVolumeSlider.value != gsm.SFXVolume;

        if (hasChanges)
            ExitOptionsMenu();
        else
            TogglePanel();
    }

    public void ExitOptionsMenu()
    {
        _optionsMenuExit.SetActive(true);
        _optionMenu.SetActive(false);
    }

    public void CancelExit()
    {
        _optionsMenuExit.SetActive(false);
        _optionMenu.SetActive(true);
    }

    public void ConfirmExitToMainMenu()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void TogglePanel()
    {
        OptionsMenu.Instance?.ToggleOptionsMenu();
    }

    public void OpenCreditsMenu()
    {
        _creditsMenu.SetActive(true);
        _optionMenu.SetActive(false);
    }

    private void OnDestroy()
    {
        _brightSlider.onValueChanged.RemoveAllListeners();
        _musicVolumeSlider.onValueChanged.RemoveAllListeners();
        _sfxVolumeSlider.onValueChanged.RemoveAllListeners();
    }
}
