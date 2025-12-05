using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerInteraction playerInteraction = GetComponentInParent<PlayerInteraction>();
        var interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            playerInteraction.interactTargetList.Add(interactable);
            playerInteraction.UpdateFocusedTarget();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        PlayerInteraction playerInteraction = GetComponentInParent<PlayerInteraction>();
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
