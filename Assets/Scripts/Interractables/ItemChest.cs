using UnityEngine;
using UnityEngine.InputSystem;

public class Chest : MonoBehaviour , IInteractable
{
    public ItemData itemInside;           // Welches Item gibt die Kiste raus?
    public GameObject pickupPrefab;       // Dein Universal-Pickup (PickupItem)

    private bool opened = false;

     public void Interact(GameObject player)
    {
        // PlayerInput holen
        var input = player.GetComponent<PlayerInput>();
        if (input == null) return;

        int sessionIndex = input.playerIndex + 1; // 0 → 1, 1 → 2

        OpenChest(sessionIndex);
    }
    public void OpenChest(int playerIndex)
    {
        if (opened) return;
        opened = true;

        // Item Pickup spawnen
        var pickup = Instantiate(pickupPrefab, transform.position, Quaternion.identity);

        // PickupItem bekommt das Item inside
        var pi = pickup.GetComponent<PickupItem>();
        pi.item = itemInside;

        // Optional: Visuelle Änderung
        // z.B. Sprite wechseln, Animator triggern usw.

        Debug.Log("Chest opened by Player " + playerIndex);
    }
}
