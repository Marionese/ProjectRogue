using UnityEngine;

public abstract class BasePickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    public bool destroyOnPickup = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnPickup(other.gameObject);

            if (destroyOnPickup)
                Destroy(gameObject);
        }
    }

    protected abstract void OnPickup(GameObject player);
}
