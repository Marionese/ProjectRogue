using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class Chest : MonoBehaviour, IInteractable
{
    public ItemData[] itemPool;           // Welches Item gibt die Kiste raus?
    public GameObject pickupPrefab;       // Dein Universal-Pickup (PickupItem)

    private bool opened = false;

    public void Interact(GameObject player)
    {
        // PlayerInput holen
        var input = player.GetComponent<PlayerInput>();
        if (input == null) return;

        OpenChest(input.playerIndex);
    }
    public void OpenChest(int playerIndex)
    {
        if (opened) return;
        opened = true;

        // Item Pickup spawnen
        var pickup = Instantiate(pickupPrefab, transform.position, Quaternion.identity);

        // PickupItem bekommt das Item inside
        var pi = pickup.GetComponent<Item>();
        ItemData chosenItem = GameSession.Instance.PickItemFromRunPool(); ;
        if (chosenItem == null)
        {
            Debug.Log("Keine Items mehr im Pool!");
            return;
        }
        pi.item = chosenItem;

        // Optional: Visuelle Änderung
        // z.B. Sprite wechseln, Animator triggern usw.

        Debug.Log("Chest opened by Player " + playerIndex);
    }
}
