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
    [SerializeField] private AudioClip jumpSfx;
    [SerializeField] private AudioSource audioSource;

    [Header("Animation")]
    [SerializeField] private Animator animator; // referencia al Animator del hijo

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

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        // Si no está asignado en el inspector, busca el Animator en hijos
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        float horiz = float.IsNaN(injectedHorizontal) ? Input.GetAxisRaw("Horizontal") : injectedHorizontal;
        Move(horiz);

        if (Input.GetButtonDown("Jump"))
            TryJump();

        UpdateAnimator(horiz);
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

        float horiz = float.IsNaN(injectedHorizontal) ? Input.GetAxisRaw("Horizontal") : injectedHorizontal;
        float moveInput = Mathf.Clamp(horiz, -1f, 1f);
        if (invertedControls) moveInput *= -1f;

        Vector2 jumpForceVec = new Vector2(moveInput * (speed * 0.5f), jumpForce);
        rb.AddForce(jumpForceVec, ForceMode2D.Impulse);

        if (audioSource != null && jumpSfx != null)
            audioSource.PlayOneShot(jumpSfx);

        isGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    private void UpdateAnimator(float horizInput)
    {
        if (animator == null) return;

        float speedX = Mathf.Abs(rb.linearVelocity.x);
        bool isWalking = speedX > 0.1f && isGrounded;
        bool isJumping = !isGrounded;

        animator.SetBool("Walk", isWalking);
        animator.SetBool("Jump", isJumping);

        // Idle implícito ni caminando ni saltando
        animator.SetBool("Idle", !isWalking && !isJumping);
    }

    public void InjectHorizontalInput(float value) => injectedHorizontal = value;
    public void ClearInjectedInput() => injectedHorizontal = float.NaN;
    public void SetInvertedControls(bool value) => invertedControls = value;
}
