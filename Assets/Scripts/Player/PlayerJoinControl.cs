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
        // Ignore join event if this is the pre-existing Player 1
        if (newPlayer == player1InScene)
            return;
        // Assign Gamepad only to Player 2+
        if (newPlayer.devices.Count > 0)
        {
            GameSession.Instance.JoinCoop();
            Destroy(playerDummy); //DestroyDummy
            newPlayer.SwitchCurrentControlScheme("Gamepad", newPlayer.devices[0]);
            newPlayer.GetComponent<PlayerController>().SetPlayerID(1);
            newPlayer.transform.SetParent(playersParent);
        }
        else
            Debug.LogWarning("Second player tried to join without a controller!");
    }
}
