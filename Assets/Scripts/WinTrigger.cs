using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WinTrigger : MonoBehaviour
{
    private void Reset()
    {
        // Asegura que el collider sea trigger
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LevelManager.Instance?.TriggerWin();
        }
    }
}
