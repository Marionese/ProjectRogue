using UnityEngine;

[System.Serializable]
public class PlayerRuntimeStats
{
    public float moveSpeed;
    public float jumpForce;
    public bool hasDoubleJump;
    public float gravity;
    public float fallGravityMultiplier;
    public float lowJumpGravityMultiplier;
    public float coyoteTime;

    // Nested snapshot struct
    [System.Serializable]
    public struct Snapshot
    {
        public int hp;
        public int maxHP;
        public float moveSpeed;
        public float jumpForce;
        public float gravity;
        // etc...
    }

    public Snapshot CreateSnapshot()
    {
        return new Snapshot
        {
            moveSpeed = moveSpeed,
            jumpForce = jumpForce,
            gravity = gravity
        };
    }

    public void ApplySnapshot(Snapshot snap)
    {
        moveSpeed = snap.moveSpeed;
        jumpForce = snap.jumpForce;
        gravity = snap.gravity;
    }

    public PlayerRuntimeStats(PlayerBaseStats baseStats)
    {
        moveSpeed = baseStats.moveSpeed;
        jumpForce = baseStats.jumpForce;
        hasDoubleJump = baseStats.hasDoubleJump;
        gravity = baseStats.gravity;
        fallGravityMultiplier = baseStats.fallGravityMultiplier;
        lowJumpGravityMultiplier = baseStats.lowJumpGravityMultiplier;
        coyoteTime = baseStats.coyoteTime;

    }
}
