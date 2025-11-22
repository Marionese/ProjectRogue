using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed = 5f;
    InputAction moveAction;
    private GameObject currentPlattform;
    private BoxCollider2D playerColider;
    InputAction jumpAction;
    InputAction dropAction;
    private float jumpforce = 15f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    private Boolean isGrounded;
    public float coyoteTime = 0.15f;
    private float coyoteTimeCounter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        rb.linearVelocity = new Vector2(moveValue.x * moveSpeed, rb.linearVelocityY);
        //check coyote
        checkGrounded();
        if (isGrounded)
        {
            Debug.Log("i am grounded");
        }

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
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
        if (jumpAction.WasPressedThisFrame() && coyoteTimeCounter > 0f && rb.linearVelocity.y <= 0f)
        {
            isGrounded = false;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpforce);

            Debug.Log("i jump");
            coyoteTimeCounter = 0f;

        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
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

