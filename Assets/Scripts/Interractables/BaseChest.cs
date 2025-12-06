using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class BaseChest : MonoBehaviour, IInteractable
{
    public GameObject pickupPrefab;
    public Transform itemSpawnPoint;
    public int playerFocused;
    [SerializeField] private Sprite chestOpened;
    protected bool opened = false;
    SpriteRenderer sr;
    public Vector3 Position => transform.position;
    [SerializeField] private GameObject highlightIcon;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    public void Interact(GameObject player)
    {
        var input = player.GetComponent<PlayerInput>();
        if (input == null) return;

        if (!opened)
        {
            sr.sprite = chestOpened;
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
        highlightIcon.SetActive(false);
        if (!GameSession.Instance.twoPlayer)
        {
            SpawnOneItem();
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
    //Set Highlight Icon active if 1 or more players are near
    public void SetHighlight(bool state)
    {
        if (state)
            playerFocused++;
        else
            playerFocused = Mathf.Max(0, playerFocused - 1);

        highlightIcon.SetActive(!opened&&playerFocused >0);
    }
}

