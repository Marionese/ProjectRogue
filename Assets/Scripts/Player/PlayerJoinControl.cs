using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoinControl : MonoBehaviour
{
    [SerializeField] private GameObject playerDummy;
    [SerializeField] private PlayerInput player1InScene;   // Assign in inspector
    
    public Transform playersParent;
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
        // Ignore pre-existing Player1
        if (newPlayer == player1InScene)
            return;

        // Check if the joining device is a GAMEPAD
        bool joinedWithGamepad = false;
        foreach (var dev in newPlayer.devices)
        {
            if (dev is Gamepad)
            {
                joinedWithGamepad = true;
                break;
            }
        }

        // If NOT gamepad → Reject the join attempt instantly
        if (!joinedWithGamepad)
        {
            Debug.Log("Rejected Player 2 join: must use a gamepad.");
            Destroy(newPlayer.gameObject);   // IMPORTANT: destroy the accidental player object
            return;
        }

        // At this point, we know it's a gamepad join
        GameSession.Instance.JoinCoop();
        Destroy(playerDummy);

        newPlayer.SwitchCurrentControlScheme("Gamepad", newPlayer.devices[0]);
        newPlayer.GetComponent<PlayerController>().SetPlayerID(1);
        newPlayer.transform.SetParent(playersParent);
    }

}