using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : UIManager
{
    [Header("Panels & Exit")]
    [SerializeField] private GameObject _exitPanel;
    [SerializeField] private List<GameObject> _buttonsToDisableOnExit;

    [Header("Options Menu")]
    [SerializeField] private GameObject _optionsMenuPrefab;
    private GameObject _optionsMenuInstance;
    [SerializeField] private List<GameObject> _buttonsToDisableWhenOptionsOpen;

    private bool _isOptionsMenuOpen = false;

    protected override void Awake()
    {
        base.Awake();

        if (_exitPanel != null) _exitPanel.SetActive(false);
        _optionsMenuInstance = Instantiate(_optionsMenuPrefab, _mainCanvas.transform);
        _optionsMenuInstance.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ToggleExitPanel()
    {
        bool show = !_exitPanel.activeSelf;
        _exitPanel.SetActive(show);
        foreach (var btn in _buttonsToDisableOnExit)
        {
            if (btn != null) btn.SetActive(!show);
        }
    }

    // --- Funcionalidad del menú de opciones ---
    public void ToggleOptionsMenu()
    {

        _isOptionsMenuOpen = !_optionsMenuInstance.activeSelf;
        _optionsMenuInstance.SetActive(_isOptionsMenuOpen);

        foreach (var btn in _buttonsToDisableWhenOptionsOpen)
        {
            if (btn != null)
                btn.SetActive(!_isOptionsMenuOpen);
        }
    }
}
