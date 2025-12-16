using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerRuntimeStats stats;

    // Input still comes as Vector2 (X,Z)
    private Vector2 moveInput;

    public void Initialize(PlayerRuntimeStats runtimeStats)
    {
        stats = runtimeStats;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.useGravity = false;

        // Prevent physics from tipping the player over
        rb.constraints = RigidbodyConstraints.FreezeRotationX
                       | RigidbodyConstraints.FreezeRotationZ
                       | RigidbodyConstraints.FreezeRotationY;
    }

    void Update()
    {
        if (stats == null) return;

        Vector3 moveDir = new Vector3(
            moveInput.x,
            0f,
            moveInput.y
        );

        rb.linearVelocity = moveDir * stats.moveSpeed;
    }

    // Called from PlayerController
    public void SetMoveInput(Vector2 input)
    {
        moveInput = input.normalized;
    }

    // Still unused (kept for API compatibility)
    public void SetJump(bool pressed) { }
}
