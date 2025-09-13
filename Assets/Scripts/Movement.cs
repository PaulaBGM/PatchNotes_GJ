using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 7f;

    [Header("Level Settings")]
    [SerializeField] private bool invertedControls = false; // Enable in "good" level

    private Rigidbody2D rb;
    private bool isGrounded;

    // External input injection (x = -1..1). If null, uses legacy Input.
    private float injectedHorizontal = float.NaN;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // Important: freeze rotation to avoid tipping if using dynamic physics
        rb.freezeRotation = true;
    }

    private void Update()
    {
        // Read inputs in Update (input timing)
        float horiz = float.IsNaN(injectedHorizontal) ? Input.GetAxisRaw("Horizontal") : injectedHorizontal;
        Move(horiz);

        // Jump input kept as legacy for simplicity; remains consistent across levels
        if (Input.GetButtonDown("Jump"))
            TryJump();
    }

    // Public Move overload that accepts external horizontal input (useful for Input System)
    public void Move(float rawHorizontal)
    {
        // Normalize to -1/0/1 from legacy axes
        float moveInput = Mathf.Clamp(rawHorizontal, -1f, 1f);

        if (invertedControls)
            moveInput *= -1f;

        // Apply X velocity; preserve Y
        Vector2 v = rb.linearVelocity;
        v.x = moveInput * speed;
        rb.linearVelocity = v;
    }

    private void TryJump()
    {
        if (!isGrounded) return;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    // Allow external systems (e.g. an Input System wrapper) to push horizontal input
    public void InjectHorizontalInput(float value)
    {
        injectedHorizontal = value;
    }

    public void ClearInjectedInput()
    {
        injectedHorizontal = float.NaN;
    }

    // Called by LevelManager to set inverted controls
    public void SetInvertedControls(bool value) => invertedControls = value;
}
