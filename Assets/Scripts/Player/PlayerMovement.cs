using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerRuntimeStats stats;

    private Vector2 moveInput;

    public void Initialize(PlayerRuntimeStats runtimeStats)
    {
        stats = runtimeStats;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Top-down: no gravity
        rb.gravityScale = 0;

        // No physics rotation
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (stats == null) return;

        // Movement ONLY in X/Y plane
        // No jump, no ground, no gravity
        rb.linearVelocity = moveInput * stats.moveSpeed;
    }

    // Called from PlayerController
    public void SetMoveInput(Vector2 input)
    {
        moveInput = input.normalized; // normalized for consistent speed
    }
    
}
