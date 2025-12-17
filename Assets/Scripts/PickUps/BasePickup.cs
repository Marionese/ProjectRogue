using UnityEngine;

public abstract class BasePickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    public bool destroyOnPickup = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject player = other.GetComponent<PlayerController>().gameObject;
            OnPickup(player);

            if (destroyOnPickup)
                Destroy(gameObject);
        }
    }

    protected abstract void OnPickup(GameObject player);
}
