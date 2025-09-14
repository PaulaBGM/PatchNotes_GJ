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
    [SerializeField] private bool isGoodLevel = false;

    [Header("Lose Settings")]
    [SerializeField] private float loseCanvasDelay = 1.5f;
    [SerializeField] private float returnToMenuDelay = 2f;
    [SerializeField] private AudioClip deathSfx;
    [SerializeField] private AudioSource audioSource;

    public bool IsGoodLevel => isGoodLevel;
    public int BrokenLevelIndex => MenuController.Instance != null ? MenuController.Instance.BrokenLossCount : 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        ConfigureLevel();
    }

    private void ConfigureLevel()
    {
        if (MenuController.Instance != null)
            isGoodLevel = MenuController.Instance.NextLevelIsGood;

        if (player != null)
            player.SetInvertedControls(isGoodLevel);

        // Activar agujeros solo si el nivel no es el bueno
        for (int i = 0; i < holes.Length; i++)
            holes[i].SetActive(!isGoodLevel && i == 0);
    }

    public void PlayerDefeated(string reason)
    {
        Debug.Log(reason);

        MenuController.Instance?.OnPlayerDeath();

        if (audioSource != null && deathSfx != null)
            audioSource.PlayOneShot(deathSfx);

        if (MusicManager.Instance != null && MenuController.Instance != null)
        {
            AudioClip gameOverClip = MenuController.Instance.GetGameOverMusic();
            if (gameOverClip != null)
                MusicManager.Instance.PlayMusic(gameOverClip, 1f);
        }

        Invoke(nameof(ShowLoseCanvas), loseCanvasDelay);
        Invoke(nameof(ReturnToMenu), loseCanvasDelay + returnToMenuDelay);
    }

    private void ShowLoseCanvas()
    {
        if (loseCanvas != null)
            loseCanvas.SetActive(true);
    }

    public void PlayerWon()
    {
        if (winCanvas != null)
            winCanvas.SetActive(true);
    }

    public void TriggerWin()
    {
        Debug.Log("Jugador lleg� al objetivo. Victoria.");
        PlayerWon();
    }

    private void ReturnToMenu() => SceneManager.LoadScene(0);
}
