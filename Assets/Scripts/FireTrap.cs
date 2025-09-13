using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FireTrap : MonoBehaviour
{
    [SerializeField] private Animator fireAnimator; // Animador del hijo con fuego
    [SerializeField] private string fireTriggerName = "Activate"; // Nombre del trigger en el Animator

    private bool activated = false;

    private void Reset()
    {
        // Autoasigna el Animator del hijo si existe
        if (fireAnimator == null)
            fireAnimator = GetComponentInChildren<Animator>();

        // Asegurarse de que el collider es trigger
        var col = GetComponent<Collider2D>();
        if (col != null) col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activated) return;

        if (other.CompareTag("Player"))
        {
            activated = true;

            // Activar animación de fuego
            if (fireAnimator != null && !string.IsNullOrEmpty(fireTriggerName))
                fireAnimator.SetTrigger(fireTriggerName);

            // Matar al jugador
            LevelManager.Instance?.PlayerDefeated("El jugador fue quemado por fuego");
        }
    }
}
