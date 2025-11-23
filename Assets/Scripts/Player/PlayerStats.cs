using UnityEngine;

[CreateAssetMenu(menuName = "Player/Player Stats")]
public class PlayerStats : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Jumping")]
    public float jumpForce = 15f;
    public bool hasDoubleJump = true;

    [Header("Gravity")]
    public float gravity = 4.5f;
    public float fallGravityMultiplier = 2.2f;
    public float lowJumpGravityMultiplier = 2f;

    [Header("Coyote Time")]
    public float coyoteTime = 0.15f;
}
