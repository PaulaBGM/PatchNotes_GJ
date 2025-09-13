using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public static MenuController Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private Button[] brokenButtons; // Botones rotos: Difícil, Normal, Fácil
    [SerializeField] private Button goodButton;      // Botón del nivel bueno

    private int brokenLossCount;

    private void Awake()
    {
        // Singleton para que solo exista uno
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (mainMenuCanvas != null) mainMenuCanvas.SetActive(true);

        // Solo mostrar el primer botón roto al inicio
        for (int i = 0; i < brokenButtons.Length; i++)
            brokenButtons[i].gameObject.SetActive(i == 0);

        if (goodButton != null) goodButton.gameObject.SetActive(false);

        // Configurar botones rotos
        for (int i = 0; i < brokenButtons.Length; i++)
        {
            int idx = i;
            brokenButtons[i].onClick.AddListener(() => OnLevelSelected(idx));
        }

        // Configurar botón bueno
        if (goodButton != null)
            goodButton.onClick.AddListener(OnGoodButton);
    }

    // Se llama al pulsar un botón roto
    public void OnLevelSelected(int idx)
    {
        SceneManager.LoadScene(1); // carga nivel roto
    }

    // Se llama desde LevelManager cuando el jugador pierde
    public void OnPlayerDeath()
    {
        brokenLossCount++;
        Debug.Log("OnPlayerDeath llamado. Total derrotas: " + brokenLossCount);

        if (brokenLossCount < brokenButtons.Length)
        {
            brokenButtons[brokenLossCount].gameObject.SetActive(true);
        }
        else if (brokenLossCount >= brokenButtons.Length && goodButton != null)
        {
            goodButton.gameObject.SetActive(true);
        }
    }


    public void OnGoodButton()
    {
        SceneManager.LoadScene(1); // Nivel correcto con controles invertidos
    }
}
