using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class BaseChest : MonoBehaviour, IInteractable
{
    public GameObject pickupPrefab;
    public Transform itemSpawnPoint;
    protected bool opened = false;

    public void Interact(GameObject player)
    {
        var input = player.GetComponent<PlayerInput>();
        if (input == null) return;

        if (!opened)
        {
            opened = true;
            OnChestOpened();
        }
    }

    protected void SpawnItem(ItemData item, Vector2 position)
    {
        // Spawn direkt auf der Chest
        var pickup = Instantiate(pickupPrefab, position, Quaternion.identity);
        pickup.GetComponent<Item>().item = item;
    }

    // Child entscheidet über den Pool
    protected abstract ItemData DecideItem();

    protected virtual void OnChestOpened()
    {
        if (!GameSession.Instance.twoPlayer)
        {
            ItemData chosen = DecideItem();

            if (chosen == null)
            {
                Debug.Log("No item available for this chest.");
                return;
            }

            SpawnItem(chosen, itemSpawnPoint.position);
        }
        else if (GameSession.Instance.twoPlayer)
        {
            {
                ItemData chosen1 = DecideItem();
                ItemData chosen2 = DecideItem();
                switch ((chosen1, chosen2))
                {
                    case (null, null): 
                    Debug.Log("No Items Left in Pool");
                    break;
                    case (not null,null):
                    SpawnItem(chosen1,itemSpawnPoint.position);
                    Debug.Log("No Items Left in Pool for two items");
                    break;
                    case (not null,not null): 
                    SpawnItem(chosen1,itemSpawnPoint.position);
                    Vector2 temp = itemSpawnPoint.position;
                    temp.x += 4;
                    SpawnItem(chosen2,temp);
                    break;
                }


            }
        }
    }
}
