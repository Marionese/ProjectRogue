using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class BaseChest : MonoBehaviour, IInteractable
{
    public GameObject pickupPrefab;
    public Transform itemSpawnPoint;
    protected bool opened = false;
    public Vector3 Position => transform.position;
    [SerializeField] private GameObject highlightIcon;
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
            SpawnOneItem();
            highlightIcon.SetActive(false);
            return;
        }

        SpawnTwoItems();
    }

    void SpawnOneItem()
    {
        var itemChosen = DecideItem();
        if (itemChosen != null)
        {
            SpawnItem(itemChosen, itemSpawnPoint.position);
        }
    }

    void SpawnTwoItems()
    {
        var itemChosen1 = DecideItem();
        var ItemChosen2 = DecideItem();

        if (itemChosen1 == null && ItemChosen2 == null)
        {
            Debug.Log("No Item Left in Pool");
            return;
        }
        if (itemChosen1 != null)
        {
            SpawnItem(itemChosen1, itemSpawnPoint.position);
        }
        if (ItemChosen2 != null)
        {
            Vector2 offsetItemPostion = itemSpawnPoint.position;
            offsetItemPostion.x += 3.5f;
            SpawnItem(ItemChosen2, offsetItemPostion);
        }
    }
    public void SetHighlight(bool state)
    {
        if (!opened)
            highlightIcon.SetActive(state);
    }
}

