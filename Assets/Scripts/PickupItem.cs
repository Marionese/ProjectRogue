using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public ItemData item;

    private void OnTriggerEnter2D(Collider2D col)
    {
        var pm = col.GetComponent<PlayerModifierManager>();
        if (pm)
        {
            pm.AddItem(item);
            Destroy(gameObject);
        }
    }
}
