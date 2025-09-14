using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class KillZone : MonoBehaviour
{
    [SerializeField] private string deathReason = "El jugador cayó en un hueco";

    private void Reset()
    {
        // Aseguramos que el collider sea un trigger
        var col = GetComponent<Collider2D>();
        if (col != null) col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LevelManager.Instance?.PlayerDefeated(deathReason);
        }
    }
}
