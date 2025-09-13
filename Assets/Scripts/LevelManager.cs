using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Movement player;
    [SerializeField] private Tomb tomb;
    [SerializeField] private GameObject[] holes;
    [SerializeField] private GameObject winCanvas;
    [SerializeField] private GameObject loseCanvas;

    [Header("Difficulty Settings")]
    [SerializeField] private bool isGoodLevel = false; // nivel correcto
    [SerializeField] private int easyExtraHoles = 1;

    private void Awake() => Instance = this;

    private void Start() => ConfigureLevel();

    private void ConfigureLevel()
    {
        if (isGoodLevel)
        {
            player.SetInvertedControls(true);
            tomb.Activate(false);
        }
        else
        {
            player.SetInvertedControls(false);
            tomb.Activate(true);
        }

        for (int i = 0; i < holes.Length; i++)
            holes[i].SetActive(!isGoodLevel && i < (isGoodLevel ? 0 : 1 + easyExtraHoles));
    }

    public void PlayerDefeated(string reason)
    {
        Debug.Log(reason);
        if (loseCanvas != null) loseCanvas.SetActive(true);
        Invoke(nameof(ReturnToMenu), 2f);
    }

    public void PlayerWon()
    {
        if (winCanvas != null) winCanvas.SetActive(true);
    }

    private void ReturnToMenu() => SceneManager.LoadScene(0);
}
