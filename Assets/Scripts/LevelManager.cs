using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Movement player;
    [SerializeField] private GameObject[] holes;
    [SerializeField] private GameObject winCanvas;
    [SerializeField] private GameObject loseCanvas;

    [Header("Difficulty Settings")]
    [SerializeField] private bool isGoodLevel = false; // Nivel correcto
    [SerializeField] private int easyExtraHoles = 1;

    private void Awake()
    {
        // Singleton solo si quieres accederlo globalmente en un nivel
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start() => ConfigureLevel();

    private void ConfigureLevel()
    {
        if (player != null)
            player.SetInvertedControls(isGoodLevel);

        for (int i = 0; i < holes.Length; i++)
            holes[i].SetActive(!isGoodLevel && i < (isGoodLevel ? 0 : 1 + easyExtraHoles));
    }

    public void PlayerDefeated(string reason)
    {
        Debug.Log(reason);

        if (loseCanvas != null) loseCanvas.SetActive(true);

        // Avisar al menú solo si existe
        if (MenuController.Instance != null)
            MenuController.Instance.OnPlayerDeath();
        else
            Debug.LogWarning("MenuController.Instance es null, no se puede desbloquear botones.");

        // Volver al menú después de 2 segundos
        Invoke(nameof(ReturnToMenu), 2f);
    }

    public void PlayerWon()
    {
        if (winCanvas != null) winCanvas.SetActive(true);
    }

    private void ReturnToMenu() => SceneManager.LoadScene(0);
}
