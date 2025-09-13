using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public static MenuController Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private Button[] levelButtons;
    [SerializeField] private Button goodButton;

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
        DontDestroyOnLoad(gameObject); // Mantener entre escenas
    }

    private void Start()
    {
        if (mainMenuCanvas != null) mainMenuCanvas.SetActive(true);
        if (goodButton != null) goodButton.gameObject.SetActive(false);

        if (levelButtons != null)
        {
            for (int i = 0; i < levelButtons.Length; i++)
            {
                int idx = i;
                levelButtons[i].onClick.AddListener(() => OnLevelSelected(idx));
            }
        }

        if (goodButton != null)
            goodButton.onClick.AddListener(OnGoodButton);
    }

    public void OnLevelSelected(int idx)
    {
        brokenLossCount++;
        SceneManager.LoadScene(1);

        if (brokenLossCount >= 3 && goodButton != null)
            goodButton.gameObject.SetActive(true);
    }

    public void OnGoodButton()
    {
        // Nivel correcto con controles invertidos
        SceneManager.LoadScene(1);
    }
}
