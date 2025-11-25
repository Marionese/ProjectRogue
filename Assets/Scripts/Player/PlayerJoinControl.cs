using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoinControl : MonoBehaviour
{
    [SerializeField] private PlayerInput player1InScene;   // Assign in inspector

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
        // Assign Gamepad only to Player 2+
        if (newPlayer.devices.Count > 0)
            newPlayer.SwitchCurrentControlScheme("Gamepad", newPlayer.devices[0]);
        else
            Debug.LogWarning("Second player tried to join without a controller!");
    }
}
