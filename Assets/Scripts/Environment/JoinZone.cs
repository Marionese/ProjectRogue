using UnityEngine;
using UnityEngine.InputSystem;

public class JoinZone : MonoBehaviour, IInteractable
{
    public bool joinEnabled;
    public Vector3 Position => transform.position;
    [SerializeField] private GameObject highlightIcon;
    public void Interact(GameObject player)
    {
        if (!joinEnabled) return;
        highlightIcon.SetActive(false);
        PlayerInputManager.instance.EnableJoining();
        Debug.Log("Co-op enabled via interact!");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("PlayerHitBox")) return;

        if (PlayerInputManager.instance == null)
            return;

        // enable new players to join
        joinEnabled = true;

    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("PlayerHitBox")) return;

        if (PlayerInputManager.instance == null)
            return;

        joinEnabled = false;
    }
    public void SetHighlight(bool state)
    {
        if (!PlayerInputManager.instance.joiningEnabled)
            highlightIcon.SetActive(state);
    }



}
