using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 7f;

    [Header("Level Settings")]
    [SerializeField] private bool invertedControls = false; // Enable in "good" levels

    private Rigidbody2D rb;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        // Horizontal input
        float moveInput = Input.GetAxisRaw("Horizontal"); // -1 (left) | 0 | 1 (right)

        // Invert controls if needed
        if (invertedControls)
            moveInput *= -1;

        // Set velocity on X axis
        Vector2 velocity = rb.linearVelocity;
        velocity.x = moveInput * speed;
        rb.linearVelocity = velocity;
    }

    private void Jump()
    {
        // Jump only if grounded
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Detect if touching the ground
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    // Public method to change control settings (used by LevelManager, for example)
    public void SetInvertedControls(bool value)
    {
        invertedControls = value;
    }
}
