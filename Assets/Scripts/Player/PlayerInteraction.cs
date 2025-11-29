using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private readonly List<IInteractable> interactTargetList = new();
    private IInteractable currentFocused;

    public void Interact(GameObject player)
    {
        currentFocused?.Interact(player);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            interactTargetList.Add(interactable);
            UpdateFocusedTarget();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        var interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            interactTargetList.Remove(interactable);

            if (currentFocused == interactable)
            {
                UpdateFocusedTarget();
            }
        }
    }

    void UpdateFocusedTarget()
    {
        // altes Highlight aus
        if (currentFocused != null)
            currentFocused.SetHighlight(false);

        IInteractable newFocus = null;
        float closestDist = Mathf.Infinity;
        Vector3 playerPos = transform.position;

        foreach (var target in interactTargetList)
        {
            if (target == null) continue;

            float dist = Vector3.Distance(playerPos, target.Position);
            if (dist < closestDist)
            {
                closestDist = dist;
                newFocus = target;
            }
        }

        currentFocused = newFocus;

        if (currentFocused != null)
            currentFocused.SetHighlight(true);
    }
}
