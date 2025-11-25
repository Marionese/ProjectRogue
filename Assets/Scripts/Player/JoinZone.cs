using UnityEngine;
using UnityEngine.InputSystem;

public class JoinZone : MonoBehaviour , IInteractable
{
    public bool joinEnabled;

      public void Interact(GameObject player)
    {
        if (!joinEnabled) return;

        PlayerInputManager.instance.EnableJoining();
        Debug.Log("Co-op enabled via interact!");
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (PlayerInputManager.instance == null)
            return;

        // enable new players to join
        joinEnabled = true;

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (PlayerInputManager.instance == null)
            return;

        joinEnabled = false;
    }

   
}
