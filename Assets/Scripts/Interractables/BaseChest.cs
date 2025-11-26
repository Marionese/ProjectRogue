using UnityEngine;
using UnityEngine.InputSystem;

public abstract class BaseChest : MonoBehaviour, IInteractable
{
    public GameObject pickupPrefab;

    protected bool opened = false;

    public void Interact(GameObject player)
    {
        var input = player.GetComponent<PlayerInput>();
        if (input == null) return;

        if (!opened)
        {
            opened = true;
            OnChestOpened(input.playerIndex);
        }
    }

    protected void SpawnItem(ItemData item)
    {
        // Spawn direkt auf der Chest
        var pickup = Instantiate(pickupPrefab, transform.position, Quaternion.identity);
        pickup.GetComponent<Item>().item = item;
    }

    // Child entscheidet über den Pool
    protected abstract ItemData DecideItem(int playerIndex);

    protected virtual void OnChestOpened(int playerIndex)
    {
        ItemData chosen = DecideItem(playerIndex);

        if (chosen == null)
        {
            Debug.Log("No item available for this chest.");
            return;
        }

        SpawnItem(chosen);
    }
}
