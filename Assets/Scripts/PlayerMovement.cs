using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Jumping")]
    public float jumpForce = 12f;
    public float coyoteTime = 0.2f;
    public float jumpBufferTime = 0.15f;
    public float fallSpeedClamp = -20f;
    public float apexBonus = 1f;
    public float apexThreshold = 1f;

    [Header("Ground & Ledge Checks")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public Transform ledgeCheck;
    public float ledgeCheckRadius = 0.1f;
    public LayerMask ledgeLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput;
    private float coyoteCounter;
    private float jumpBufferCounter;
    private bool isJumping;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private PlayerLantern playerLantern;
    private AudioSource audioSource;
    private PlayerLantern lantern;

    private bool wasGroundedLastFrame = false;

    public AudioClip fallLandingSound;
    public AudioClip fallLandingLanternSound;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponentInChildren<AudioSource>();
        playerLantern = GetComponent<PlayerLantern>();
        lantern = GetComponent<PlayerLantern>();
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

        // Coyote Time
        if (isGrounded)
            coyoteCounter = coyoteTime;
        else
            coyoteCounter -= Time.deltaTime;

        // Jump Buffer
        if (Input.GetButtonDown("Jump"))
            jumpBufferCounter = jumpBufferTime;
        else
            jumpBufferCounter -= Time.deltaTime;

        // Jump
        if (jumpBufferCounter > 0 && coyoteCounter > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isJumping = true;
            jumpBufferCounter = 0;
        }

        // Variable Jump Height
        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0 && isJumping)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            isJumping = false;
        }

        // Apex Modifier
        if (Mathf.Abs(rb.linearVelocity.y) < apexThreshold)
        {
            rb.linearVelocity = new Vector2(moveInput * (moveSpeed + apexBonus), rb.linearVelocity.y);
        }

        // Clamp Fall Speed
        if (rb.linearVelocity.y < fallSpeedClamp)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fallSpeedClamp);
        }
        
        // Flip Sprite
            if (moveInput != 0)
            {
                spriteRenderer.flipX = moveInput < 0;
            }

        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("VerticalVel", rb.linearVelocity.y);
        animator.SetBool("LanternEquipped", playerLantern?.IsLanternEquipped() ?? false);
        animator.SetBool("HasLantern", playerLantern?.HasLantern() ?? false);

        if (!wasGroundedLastFrame && isGrounded)
        {
            if (lantern != null && lantern.IsLanternEquipped())
                audioSource.PlayOneShot(fallLandingLanternSound);
            else
                audioSource.PlayOneShot(fallLandingSound);
        }

        wasGroundedLastFrame = isGrounded;
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

        if (ledgeCheck != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(ledgeCheck.position, ledgeCheckRadius);
        }
    }
}
