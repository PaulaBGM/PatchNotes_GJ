using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public static MenuController Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private Button[] brokenButtons; // 0 = Difícil, 1 = Normal
    [SerializeField] private Button goodButton;      // Botón del nivel bueno

    [Header("Music Clips")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip hardMusic;
    [SerializeField] private AudioClip normalMusic;
    [SerializeField] private AudioClip goodMusic;
    [SerializeField] private AudioClip gameOverMusic;

    private int brokenLossCount;
    private bool nextLevelIsGood = false;

    public bool NextLevelIsGood => nextLevelIsGood;
    public int BrokenLossCount => brokenLossCount;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        if (mainMenuCanvas != null) mainMenuCanvas.SetActive(true);

        // Mostrar solo el botón Difícil al inicio
        for (int i = 0; i < brokenButtons.Length; i++)
            brokenButtons[i].gameObject.SetActive(i == 0);

        if (goodButton != null)
            goodButton.gameObject.SetActive(false);

        for (int i = 0; i < brokenButtons.Length; i++)
        {
            int idx = i;
            brokenButtons[i].onClick.AddListener(() => OnLevelSelected(idx));
        }

        if (goodButton != null)
            goodButton.onClick.AddListener(OnGoodButton);

        PlayMenuMusic();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0) // Menú
        {
            if (mainMenuCanvas != null)
                mainMenuCanvas.SetActive(true);

            // Activar botones según número de derrotas
            for (int i = 0; i < brokenButtons.Length; i++)
                brokenButtons[i].gameObject.SetActive(i <= brokenLossCount && i < brokenButtons.Length);

            if (goodButton != null)
                goodButton.gameObject.SetActive(brokenLossCount >= brokenButtons.Length);

            PlayMenuMusic();
        }
        else // Nivel
        {
            if (mainMenuCanvas != null)
                mainMenuCanvas.SetActive(false);

            if (MusicManager.Instance != null)
            {
                AudioClip clip = GetMusicForNextLevel();
                MusicManager.Instance.PlayMusic(clip, 1f);
            }
        }
    }

    private void PlayMenuMusic()
    {
        if (MusicManager.Instance != null && menuMusic != null)
        {
            Debug.Log("Reproduciendo música de menú: " + menuMusic.name);
            MusicManager.Instance.PlayMusic(menuMusic, 1f);
        }
        else
        {
            Debug.LogWarning("No se pudo reproducir música de menú: " +
                (MusicManager.Instance == null ? "MusicManager.Instance es null " : "") +
                (menuMusic == null ? "menuMusic no asignado" : ""));
        }
    }


    public void OnLevelSelected(int idx)
    {
        nextLevelIsGood = false;
        SceneManager.LoadScene(1); // Carga escena de nivel roto
    }

    public void OnGoodButton()
    {
        nextLevelIsGood = true;
        SceneManager.LoadScene(1); // Carga escena del nivel bueno
    }

    // Solo registra derrota, no toca botones (los actualiza OnSceneLoaded al volver al menú)
    public void OnPlayerDeath()
    {
        brokenLossCount++;
        Debug.Log("OnPlayerDeath llamado. Total derrotas: " + brokenLossCount);
    }

    public AudioClip GetMusicForNextLevel()
    {
        if (nextLevelIsGood) return goodMusic;
        if (brokenLossCount == 0) return hardMusic;
        return normalMusic;
    }

    public AudioClip GetGameOverMusic() => gameOverMusic;
}
