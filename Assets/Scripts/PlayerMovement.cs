using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Jumping")]
    public float jumpForce = 12f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private PlayerLantern playerLantern;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        playerLantern = GetComponent<PlayerLantern>();
    }

    private void Update()
    {
            if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive)
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        animator.SetFloat("Speed", 0);
        return; // Freeze player input
    }

        // Movement
        moveInput = Input.GetAxisRaw("Horizontal");

        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if (moveInput != 0)
        {
            spriteRenderer.flipX = moveInput < 0;
        }

        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("VerticalVel", rb.linearVelocity.y);

        if (playerLantern != null)
        {
            animator.SetBool("LanternEquipped", playerLantern.IsLanternEquipped());
            animator.SetBool("HasLantern", playerLantern.HasLantern());
        }
        else
        {
            animator.SetBool("LanternEquipped", false);
            animator.SetBool("HasLantern", false);
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
