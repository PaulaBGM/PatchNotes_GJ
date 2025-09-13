using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Tomb : MonoBehaviour
{
    [SerializeField] private float fallSpeed = 5f;
    [SerializeField] private bool activeOnLevel = true;

    private Rigidbody2D rb;
    private bool hasFallen;

    private void Awake() => rb = GetComponent<Rigidbody2D>();

    private void Start()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        if (activeOnLevel) StartFall();
    }

    private void StartFall()
    {
        hasFallen = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = Vector2.down * fallSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasFallen) return;

        if (collision.gameObject.CompareTag("Player"))
            LevelManager.Instance.PlayerDefeated("You were crushed by the tomb!");
        else if (collision.gameObject.CompareTag("Ground"))
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    public void Activate(bool value)
    {
        activeOnLevel = value;
        if (!value)
        {
            hasFallen = false;
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
        else if (!hasFallen) StartFall();
    }
}
