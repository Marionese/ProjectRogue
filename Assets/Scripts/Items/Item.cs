using UnityEngine;
using UnityEngine.InputSystem;

public class Item : MonoBehaviour, IInteractable
{
    public ItemData item;
    public Vector3 Position => transform.position;
    [SerializeField] private GameObject highlightIcon;
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
        if (item.isUnique)
        {
            var category = item.category;

            if (category == ItemCategory.Normal)
            {
                GameSession.Instance.normalPool.Remove(item);
            }
            if (category == ItemCategory.Shop)
            {
                GameSession.Instance.shopPool.Remove(item);
            }
            if (category == ItemCategory.Boss)
            {
                GameSession.Instance.bossPool.Remove(item);
            }
        }


        // Objekt zerstören
        Destroy(gameObject);
    }
    public void SetHighlight(bool state)
    {
        highlightIcon.SetActive(state);
    }

}