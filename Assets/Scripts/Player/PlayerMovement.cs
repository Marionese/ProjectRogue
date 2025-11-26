using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
public class PlayerMovement : MonoBehaviour
{
    public PlayerStats stats;  // This is your asset reference
    private PlayerStats runtimeStats; // This will be the copy used in-game
    public PlayerStats RuntimeStats => runtimeStats;
    private Rigidbody2D rb;


    private GameObject currentPlattform;
    private BoxCollider2D playerColider;
    private Vector2 moveInput;
    private bool jumpPressed;

    private List<IInteractable> interactTargetList = new();
    private IInteractable currentFocused;


    private int jumpsLeft;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private Boolean isGrounded;
    private float jumpBuffer = 0.15f;
    private float jumpBufferCounter;
    private float coyoteTimeCounter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CameraTarget camTarget = FindFirstObjectByType<CameraTarget>();
        camTarget.Register(transform);
        runtimeStats = Instantiate(stats);
        rb = GetComponent<Rigidbody2D>();
        playerColider = GetComponent<BoxCollider2D>();
    }
    //inputs
    public void OnMove(InputValue val)
    {
        moveInput = val.Get<Vector2>();
    }

    void OnJump(InputValue val)
    {
        jumpPressed = val.isPressed;
        if (jumpPressed)
        {
            jumpBufferCounter = jumpBuffer;  // SAVE INTENTION
        }
    }

    void OnDrop()
    {
        if (currentPlattform != null)
        {
            StartCoroutine(DissableCollision());
        }

    }
    void OnInterract(InputValue val)
    {
        currentFocused?.Interact(gameObject);
    }


    // Update is called once per frame
    void Update()
    {
        //Move X accis

        rb.linearVelocity = new Vector2(moveInput.x * runtimeStats.moveSpeed, rb.linearVelocityY);
        //Check if Player is on the Ground
        checkGrounded();
        //If Player is on Ground reset jumpsleft,coyoteTime and jumpBuffer
        handleGroundEvents();
        if (jumpBufferCounter > 0f)
        {
            TryJump();
        }
        applyJumpGravity();
        jumpBufferCounter -= Time.deltaTime;

    }
    void handleGroundEvents()
    {
        if (isGrounded && rb.linearVelocity.y <= 0f)
        {

            jumpsLeft = runtimeStats.hasDoubleJump ? 2 : 1;
            coyoteTimeCounter = runtimeStats.coyoteTime;
        }
        else
        {

            coyoteTimeCounter -= Time.deltaTime;
        }
    }
    void applyJumpGravity()
    {
        if (rb.linearVelocity.y < 0f) // Fallen
        {
            rb.gravityScale = runtimeStats.gravity * runtimeStats.fallGravityMultiplier;
        }
        else if (rb.linearVelocity.y > 0f && !jumpPressed) // Früh loslassen
        {
            rb.gravityScale = runtimeStats.gravity * runtimeStats.lowJumpGravityMultiplier;
            //rb.linearVelocity = new Vector2(moveInput.x * runtimeStats.moveSpeed, rb.linearVelocity.y / 4);
        }
        else // Aufsteigend oder Boden
        {
            rb.gravityScale = runtimeStats.gravity;
        }

    }
    void TryJump()
    {
        bool canGroundJump = coyoteTimeCounter > 0f && rb.linearVelocity.y <= 0f;
        bool canDoubleJump = runtimeStats.hasDoubleJump && jumpsLeft > 1 && !isGrounded;
        if (canDoubleJump || canGroundJump)
        {
            jumpBufferCounter = 0f;
            isGrounded = false;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, runtimeStats.jumpForce);

            if (!canGroundJump)  // only use a jump if it's NOT a ground/coyote jump
            {
                jumpsLeft -= 1;
            }
            coyoteTimeCounter = 0f;
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
        Collider2D col = Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.98f, 0.05f), CapsuleDirection2D.Horizontal, 0, groundLayer);

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
        currentPlattform = null;
    }

    private IEnumerator DissableCollision()
    {
        BoxCollider2D plattformCollider = currentPlattform.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(playerColider, plattformCollider);
        yield return new WaitForSeconds(0.25f);
        Physics2D.IgnoreCollision(playerColider, plattformCollider, false);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        var interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            interactTargetList.Add(interactable);
            UpdateFocusedTarget();
        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        var interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            interactTargetList.Remove(interactable);

            // Wenn wir das fokussierte Objekt verlieren → neu bestimmen
            if (currentFocused == interactable)
            {
                UpdateFocusedTarget();
            }
        }
    }


    void UpdateFocusedTarget()
    {
        // Highlight vom alten Fokus ausschalten
        if (currentFocused != null)
            currentFocused.SetHighlight(false);

        IInteractable newFocus = null;
        float closestDist = Mathf.Infinity;
        Vector3 playerPos = transform.position;

        foreach (var target in interactTargetList)
        {
            if (target == null) continue;

            float dist = Vector3.Distance(playerPos, target.Position);
            if (dist < closestDist)
            {
                closestDist = dist;
                newFocus = target;
            }
        }

        currentFocused = newFocus;

        // Highlight vom neuen Fokus einschalten
        if (currentFocused != null)
            currentFocused.SetHighlight(true);
    }
}

