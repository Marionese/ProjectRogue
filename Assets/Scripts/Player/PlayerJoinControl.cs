using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoinControl : MonoBehaviour
{
    private int playerIndex = 0;

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

    private void OnPlayerJoined(PlayerInput player)
    {
        if (playerIndex == 0)
        {
            // Spieler 1 bekommt Tastatur + Maus (falls vorhanden) UND Controller
            
        }
        else
        {
            // Spieler 2+ bekommt NUR Controller
            if (player.devices.Count > 0)
                player.SwitchCurrentControlScheme("Gamepad", player.devices[0]);
            else
                Debug.LogWarning("Zweiter Spieler ohne Controller versucht zu joinen!");
        }

        playerIndex++;
    }
}
