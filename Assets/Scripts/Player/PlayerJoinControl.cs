using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoinControl : MonoBehaviour
{
    public float maxJoinDistance = 8f;
    [SerializeField] private PlayerInput player1InScene;   // Assign in inspector
    private Transform player1;

    void Awake()
    {
        player1 = player1InScene.transform;
    }

    void OnEnable()
    {
        if (PlayerInputManager.instance != null)
            PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
    }

    void OnDisable()
    {
        if (PlayerInputManager.instance != null)
            PlayerInputManager.instance.onPlayerJoined -= OnPlayerJoined;
    }

    private void OnPlayerJoined(PlayerInput newPlayer)
    {
        // Ignore join event if this is the pre-existing Player 1
        if (newPlayer == player1InScene)
            return;

        // Block joins too far away from Player 1
        float distance = Vector2.Distance(newPlayer.transform.position, player1.position);
        if (distance > maxJoinDistance)
        {
            Debug.Log("Join denied: too far from Player 1");
            Destroy(newPlayer.gameObject);
            return;
        }

        // Assign Gamepad only to Player 2+
        if (newPlayer.devices.Count > 0)
            newPlayer.SwitchCurrentControlScheme("Gamepad", newPlayer.devices[0]);
        else
            Debug.LogWarning("Second player tried to join without a controller!");
    }
}
