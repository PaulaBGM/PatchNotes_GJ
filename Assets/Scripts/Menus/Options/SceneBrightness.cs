using UnityEngine;

public class SceneBrightness : MonoBehaviour
{
    [SerializeField] private CanvasGroup overlay;

    private void Awake()
    {
        if (overlay == null) overlay = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        // Aplica el valor guardado
        float b = 1f;
        if (OptionsManager.Instance != null) b = OptionsManager.Instance.Data.brightness;
        Apply(b);

        if (OptionsManager.Instance != null)
            OptionsManager.Instance.OnBrightnessChanged += Apply;
    }

    private void OnDisable()
    {
        if (OptionsManager.Instance != null)
            OptionsManager.Instance.OnBrightnessChanged -= Apply;
    }

    private void Apply(float brightness01)
    {
        if (overlay == null) return;

        // Si brillo = 1 ? overlay transparente
        // Si brillo = 0 ? overlay negro
        overlay.alpha = 1f - Mathf.Clamp01(brightness01);
    }
}
