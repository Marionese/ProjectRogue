using System;
using System.Collections;
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
    private Boolean isGrounded;
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

        //Jump
        if (jumpAction.WasPressedThisFrame() && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
        }
        if (dropAction.WasPressedThisFrame())
        {
            if (currentPlattform != null)
            {
                StartCoroutine(DissableCollision());
            }

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
                    isGrounded = true;
                    break;
                }
            }

        }
        if (collision.gameObject.GetComponent<Plattform>())
        {
            currentPlattform = collision.gameObject;
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

