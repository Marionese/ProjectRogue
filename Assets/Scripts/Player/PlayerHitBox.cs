using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        PlayerInteraction playerInteraction = GetComponent<PlayerInteraction>();
        var interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            playerInteraction.interactTargetList.Add(interactable);
            playerInteraction.UpdateFocusedTarget();
        }
    }

    void OnTriggerExit(Collider other)
    {
        PlayerInteraction playerInteraction = GetComponent<PlayerInteraction>();
        var interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            playerInteraction.interactTargetList.Remove(interactable);

            if (playerInteraction.currentFocused == interactable)
            {
                playerInteraction.UpdateFocusedTarget();
            }
        }
    }
}
