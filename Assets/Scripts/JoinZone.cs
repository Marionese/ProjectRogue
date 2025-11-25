using UnityEngine;
using UnityEngine.InputSystem;

public class JoinZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (PlayerInputManager.instance == null)
            return;

        // enable new players to join
        PlayerInputManager.instance.EnableJoining();
        Debug.Log("Join enabled");


    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (PlayerInputManager.instance == null)
            return;

        PlayerInputManager.instance.DisableJoining();
        Debug.Log("Join disabled");
    }
}
