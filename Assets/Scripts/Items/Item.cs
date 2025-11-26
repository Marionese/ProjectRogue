using UnityEngine;
using UnityEngine.InputSystem;

public class Item : MonoBehaviour, IInteractable
{
    public ItemData item;

    public void Interact(GameObject player)
    {
        // PlayerInput → SessionIndex
        var input = player.GetComponent<PlayerInput>();
        if (input == null) return;

        // Spieler bekommt Modifier
        var pm = player.GetComponent<PlayerModifierManager>();
        pm.AddItem(item);

        // GameSession merkt sich das Item
        GameSession.Instance.RegisterRunItem(input.playerIndex, item);

        // Objekt zerstören
        Destroy(gameObject);
    }
}