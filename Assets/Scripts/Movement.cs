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
    private bool facingRight = true;

    // External input injection (x = -1..1). If null, uses legacy Input.
    private float injectedHorizontal = float.NaN;

    // Referencia al SpriteRenderer del hijo
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        // Asumimos que el hijo tiene el SpriteRenderer
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        // Read inputs in Update (input timing)
        float horiz = float.IsNaN(injectedHorizontal) ? Input.GetAxisRaw("Horizontal") : injectedHorizontal;
        Move(horiz);

        // Jump input
        if (Input.GetButtonDown("Jump"))
            TryJump();
    }

    public void Move(float rawHorizontal)
    {
        // Normalize to -1/0/1 from legacy axes
        float moveInput = Mathf.Clamp(rawHorizontal, -1f, 1f);

        if (invertedControls)
            moveInput *= -1f;

        // Flip sprite según dirección
        if (moveInput > 0 && !facingRight)
            Flip();
        else if (moveInput < 0 && facingRight)
            Flip();

        // Apply X velocity; preserve Y
        Vector2 v = rb.linearVelocity;
        v.x = moveInput * speed;
        rb.linearVelocity = v;
    }

    private void Flip()
    {
        facingRight = !facingRight;
        if (spriteRenderer != null)
            spriteRenderer.flipX = !facingRight;
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

    public void InjectHorizontalInput(float value)
    {
        injectedHorizontal = value;
    }

    public void ClearInjectedInput()
    {
        injectedHorizontal = float.NaN;
    }

    public void SetInvertedControls(bool value) => invertedControls = value;
}
