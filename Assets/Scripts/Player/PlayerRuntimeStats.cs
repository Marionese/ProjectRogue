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

    public int maxHP;
    public int currentHP;

    // Nested snapshot struct
    [System.Serializable]
    public struct Snapshot
    {
        public int hp;
    }

    public Snapshot CreateSnapshot()
    {
        return new Snapshot
        {
            hp = currentHP
        };
    }

    public void ApplySnapshot(Snapshot snap)
    {
        // Wenn noch nie ein Snapshot gespeichert wurde, bleibt hp = 0
        // Dann lassen wir currentHP einfach wie sie ist (maxHP)
        if (snap.hp > 0)
        {
            currentHP = Mathf.Min(snap.hp, maxHP);
        }
        else
        {
            currentHP = maxHP; // z.B. am Anfang des Runs
        }
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

        maxHP = baseStats.maxHP;       // Base Max HP
        currentHP = maxHP;

    }
}
