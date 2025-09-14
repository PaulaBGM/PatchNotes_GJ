using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 7f;

    [Header("Level Settings")]
    [SerializeField] private bool invertedControls = false;

    [Header("Audio")]
    [SerializeField] private AudioClip jumpSfx;      // Sonido del salto
    [SerializeField] private AudioSource audioSource; // Fuente de audio

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool facingRight = true;

    private float injectedHorizontal = float.NaN;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        // Si no se asignó AudioSource en el inspector, buscar uno en el mismo objeto
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        float horiz = float.IsNaN(injectedHorizontal) ? Input.GetAxisRaw("Horizontal") : injectedHorizontal;
        Move(horiz);

        if (Input.GetButtonDown("Jump"))
            TryJump();
    }

    public void Move(float rawHorizontal)
    {
        float moveInput = Mathf.Clamp(rawHorizontal, -1f, 1f);

        if (invertedControls)
            moveInput *= -1f;

        if (moveInput > 0 && !facingRight)
            Flip();
        else if (moveInput < 0 && facingRight)
            Flip();

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

        // Detecta entrada horizontal en el momento del salto
        float horiz = float.IsNaN(injectedHorizontal) ? Input.GetAxisRaw("Horizontal") : injectedHorizontal;
        float moveInput = Mathf.Clamp(horiz, -1f, 1f);
        if (invertedControls) moveInput *= -1f;

        // Aplica impulso vertical + un pequeño impulso horizontal
        Vector2 jumpForceVec = new Vector2(moveInput * (speed * 0.5f), jumpForce);
        rb.AddForce(jumpForceVec, ForceMode2D.Impulse);

        // Reproducir sonido de salto
        if (audioSource != null && jumpSfx != null)
            audioSource.PlayOneShot(jumpSfx);

        isGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    public void InjectHorizontalInput(float value) => injectedHorizontal = value;
    public void ClearInjectedInput() => injectedHorizontal = float.NaN;
    public void SetInvertedControls(bool value) => invertedControls = value;
}
