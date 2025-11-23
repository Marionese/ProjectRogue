using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    public PlayerStats stats;  // This is your asset reference
    private PlayerStats runtimeStats; // This will be the copy used in-game

    private Rigidbody2D rb;

    InputAction moveAction;
    private GameObject currentPlattform;
    private BoxCollider2D playerColider;
    InputAction jumpAction;
    InputAction dropAction;

    private int jumpsLeft;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;
    private bool isDoubleJumping;
    private Boolean isGrounded;
    private float coyoteTimeCounter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        runtimeStats = Instantiate(stats);
        rb = GetComponent<Rigidbody2D>();
        playerColider = GetComponent<BoxCollider2D>();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        dropAction = InputSystem.actions.FindAction("Drop");
    }

    // Update is called once per frame
    void Update()
    {
        //Move X accis
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        rb.linearVelocity = new Vector2(moveValue.x * runtimeStats.moveSpeed, rb.linearVelocityY);
        //check coyote
        checkGrounded();

        if (isGrounded && rb.linearVelocity.y <= 0f)
        {
            jumpsLeft = runtimeStats.hasDoubleJump ? 2 : 1;
            coyoteTimeCounter = runtimeStats.coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        //Jump



        if (dropAction.WasPressedThisFrame())
        {
            if (currentPlattform != null)
            {
                StartCoroutine(DissableCollision());
            }

        }
        bool canGroundJump = coyoteTimeCounter > 0f && rb.linearVelocity.y <= 0f;
        bool canDoubleJump = runtimeStats.hasDoubleJump && jumpsLeft > 1 && !isGrounded;
        if (jumpAction.WasPressedThisFrame() && (canDoubleJump || canGroundJump))
        {
            isGrounded = false;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, runtimeStats.jumpForce);

            isDoubleJumping = !canGroundJump;

            if (!canGroundJump)  // only use a jump if it's NOT a ground/coyote jump
            {
                jumpsLeft -= 1;
            }
            coyoteTimeCounter = 0f;

        }
        if (rb.linearVelocity.y < 0f) // Fallen
        {
            rb.gravityScale = runtimeStats.gravity * runtimeStats.fallGravityMultiplier;
        }
        else if (rb.linearVelocity.y > 0f && !jumpAction.IsPressed()) // Früh loslassen
        {
            rb.gravityScale = runtimeStats.gravity * runtimeStats.lowJumpGravityMultiplier;
        }
        else // Aufsteigend oder Boden
        {
            rb.gravityScale = runtimeStats.gravity;
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Plattform>())
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    currentPlattform = collision.gameObject;
                }
            }
        }
    }
    private void checkGrounded()
    {
        Collider2D col = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (col != null)
        {
            // only grounded if platform is under your feet, not inside it
            float platformY = col.bounds.max.y; // top of platform
            float feetY = groundCheck.position.y;

            isGrounded = feetY > platformY - 0.01f; // tiny tolerance
        }
        else
        {
            isGrounded = false;
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
        currentPlattform = null;
    }

    private IEnumerator DissableCollision()
    {
        BoxCollider2D plattformCollider = currentPlattform.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(playerColider, plattformCollider);
        yield return new WaitForSeconds(0.25f);
        Physics2D.IgnoreCollision(playerColider, plattformCollider, false);
    }

}

