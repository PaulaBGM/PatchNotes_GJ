using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;

public class OptionsMenu : MonoBehaviour
{
    public static OptionsMenu Instance;

    [SerializeField] private GameObject _optionsMenuPrefab;
    private GameObject _optionsMenu;

    [SerializeField] private GameObject _mainCanva;
    public bool IsOpen;

    [SerializeField] private List<GameObject> _buttonsToDisable;
    public float Percent;

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            StartCoroutine(InitLanguageSettings()); // << COROUTINE PARA ESPERAR
        }
        else
        {
            Destroy(gameObject);
        }

        _mainCanva = GameObject.Find("Canvas");
        IsOpen = false;
    }

    /// <summary>
    /// Espera a que LocalizationSettings esté cargado antes de acceder a las Locales
    /// </summary>
    private IEnumerator InitLanguageSettings()
    {
        yield return LocalizationSettings.InitializationOperation;

        int savedId = PlayerPrefs.GetInt("LanguageId", 0);
        var locales = LocalizationSettings.AvailableLocales.Locales;

        // Validar índice
        if (savedId < 0 || savedId >= locales.Count)
        {
            savedId = 0;
        }

        LocalizationSettings.SelectedLocale = locales[savedId];
    }

    private void Update()
    {
        if (_mainCanva == null)
        {
            CheckMainCanva();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_optionsMenu == null)
            {
                ToggleOptionsMenu();
            }
            else if (_optionsMenu.activeSelf)
            {
                ToggleOptionsMenu();
            }
            else
            {
                ToggleOptionsMenu();
            }
        }

        if (_optionsMenu != null)
        {
            if (_optionsMenu.activeSelf)
            {
                IsOpen = true;
                if (_buttonsToDisable != null && _buttonsToDisable.Count > 0)
                {
                    TurnOffAllButtons();
                }
            }
            else
            {
                IsOpen = false;
                if (_buttonsToDisable != null && _buttonsToDisable.Count > 0)
                {
                    TurnOnAllButtons();
                }
            }
        }
    }

    public void CheckMainCanva()
    {
        _mainCanva = GameObject.FindGameObjectWithTag("MainCanva");
        TurnOffAllButtons();
        GameObject levelUI = GameObject.FindGameObjectWithTag("LevelUI");
        if (levelUI != null)
            _buttonsToDisable.Add(levelUI);
    }

    private void TurnOffAllButtons()
    {
        _buttonsToDisable.RemoveAll(b => b == null);
        foreach (GameObject button in _buttonsToDisable)
        {
            if (button) button.SetActive(false);
        }
    }

    private void TurnOnAllButtons()
    {
        _buttonsToDisable.RemoveAll(b => b == null);
        foreach (GameObject button in _buttonsToDisable)
        {
            if (button) button.SetActive(true);
        }
    }

    public void ToggleOptionsMenu()
    {
        if (_optionsMenu == null)
        {
            _optionsMenu = Instantiate(_optionsMenuPrefab, _mainCanva.transform);
            _optionsMenu.SetActive(true);
        }
        else
        {
            _optionsMenu.SetActive(!_optionsMenu.activeSelf);
        }
    }
}
