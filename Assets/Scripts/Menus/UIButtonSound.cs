using UnityEngine;
using UnityEngine.UI;

public class UIButtonSound : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip clickSound;   // Sonido de todos los botones
    [SerializeField] private AudioSource audioSource; // Fuente de audio para reproducir

    private void Awake()
    {
        if (audioSource == null)
        {
            // Usa la nueva API de Unity 6
            audioSource = Object.FindFirstObjectByType<AudioSource>();
        }

        // Añadir sonido a todos los botones hijos del Canvas
        Button[] buttons = GetComponentsInChildren<Button>(true);
        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(PlayClickSound);
        }
    }

    private void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
            audioSource.PlayOneShot(clickSound);
    }
}
