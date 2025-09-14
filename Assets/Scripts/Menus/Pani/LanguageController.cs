using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System.Collections;

public class LanguageController : MonoBehaviour
{
    private int _id;

    [SerializeField]
    private TextMeshProUGUI _languageText;

    private IEnumerator Start()
    {
        // Esperar a que se inicialicen las locales
        yield return LocalizationSettings.InitializationOperation;

        // Obtener ID guardado o establecer a 0 por defecto
        _id = PlayerPrefs.GetInt("LanguageId", 0);

        // Asignar idioma seleccionado
        if (_id >= 0 && _id < LocalizationSettings.AvailableLocales.Locales.Count)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_id];
        }
        else
        {
            _id = 0;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
        }

        SetLanguageText();
    }

    public void IncreaseId()
    {
        int maxIndex = LocalizationSettings.AvailableLocales.Locales.Count - 1;

        if (_id < maxIndex)
        {
            _id++;
            ApplyLanguage();
        }
    }

    public void DecreaseId()
    {
        if (_id > 0)
        {
            _id--;
            ApplyLanguage();
        }
    }

    private void ApplyLanguage()
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_id];
        PlayerPrefs.SetInt("LanguageId", _id);
        SetLanguageText();
    }

    private void SetLanguageText()
    {
        string code = LocalizationSettings.SelectedLocale.Identifier.Code;

        _languageText.text = code switch
        {
            "es" => "Español",
            "en" => "English",
            "gl" => "Galego",
            "ca" or "ca-ES" => "Català",
            "pt" => "Português",
            _ => "Unknown"
        };
    }
}
