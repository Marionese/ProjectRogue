using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public readonly List<IInteractable> interactTargetList = new();
    public IInteractable currentFocused;

    public void Interact(GameObject player)
    {
        currentFocused?.Interact(player);
    }

    public void UpdateFocusedTarget()
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
    public void RemoveInteractable(IInteractable target)
    {
        if (currentFocused == target)
        {
            currentFocused.SetHighlight(false);
            currentFocused = null;
        }

        interactTargetList.Remove(target);
    }
}
