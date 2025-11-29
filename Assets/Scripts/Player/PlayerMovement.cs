using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D playerCollider;
    private PlayerRuntimeStats stats;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private Vector2 moveInput;
    private bool jumpPressed;

    private GameObject currentPlatform;

    private bool isGrounded;
    private int jumpsLeft;

    private float jumpBuffer = 0.15f;
    private float jumpBufferCounter;
    private float coyoteTimeCounter;

    public void Initialize(PlayerRuntimeStats runtimeStats)
    {
        stats = runtimeStats;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (stats == null) return; // Sicherheitsnetz

        // X-Bewegung
        rb.linearVelocity = new Vector2(
            moveInput.x * stats.moveSpeed,
            rb.linearVelocityY
        );

        CheckGrounded();
        HandleGroundEvents();

        if (jumpBufferCounter > 0f)
        {
            TryJump();
        }

        ApplyJumpGravity();
        jumpBufferCounter -= Time.deltaTime;
    }

    // ---------- Public API vom PlayerController ----------

    public void SetMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    public void SetJump(bool pressed)
    {
        jumpPressed = pressed;

        if (pressed)
        {
            jumpBufferCounter = jumpBuffer; // Save Intention
        }
    }

    public void DropThroughPlatform()
    {
        if (currentPlatform != null)
        {
            StartCoroutine(DisableCollision());
        }
    }

    // ---------- Movement-Interna ----------

    void HandleGroundEvents()
    {
        if (isGrounded && rb.linearVelocity.y <= 0f)
        {
            jumpsLeft = stats.hasDoubleJump ? 2 : 1;
            coyoteTimeCounter = stats.coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    void ApplyJumpGravity()
    {
        if (rb.linearVelocity.y < 0f) // Fallen
        {
            rb.gravityScale = stats.gravity * stats.fallGravityMultiplier;
        }
        else if (rb.linearVelocity.y > 0f && !jumpPressed) // Früh loslassen
        {
            rb.gravityScale = stats.gravity * stats.lowJumpGravityMultiplier;
        }
        else
        {
            rb.gravityScale = stats.gravity;
        }
    }

    void TryJump()
    {
        bool canGroundJump = coyoteTimeCounter > 0f && rb.linearVelocity.y <= 0f;
        bool canDoubleJump = stats.hasDoubleJump && jumpsLeft > 1 && !isGrounded;

        if (canGroundJump || canDoubleJump)
        {
            jumpBufferCounter = 0f;
            isGrounded = false;

            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                stats.jumpForce
            );

            if (!canGroundJump)
            {
                jumpsLeft -= 1;
            }

            coyoteTimeCounter = 0f;
        }
    }

    void CheckGrounded()
    {
        if (groundCheck == null) return;

        Collider2D col = Physics2D.OverlapCapsule(
            groundCheck.position,
            new Vector2(0.98f, 0.05f),
            CapsuleDirection2D.Horizontal,
            0,
            groundLayer
        );

        if (col != null)
        {
            float platformY = col.bounds.max.y; // Oben von der Plattform
            float feetY = groundCheck.position.y;

            isGrounded = feetY > platformY - 0.01f;
        }
        else
        {
            isGrounded = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Plattform>())
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    currentPlatform = collision.gameObject;
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        currentPlatform = null;
    }

    private IEnumerator DisableCollision()
    {
        if (currentPlatform == null) yield break;

        BoxCollider2D platformCollider = currentPlatform.GetComponent<BoxCollider2D>();
        if (platformCollider == null) yield break;

        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(0.25f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }
}
