using UnityEngine;

public class MetaCoinPickup : MonoBehaviour
{
    public int amount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameSession.Instance.metaCoins += amount;

            // Sofort speichern
            SaveSystem.SaveGame(GameSession.Instance.currentSlot);

            Debug.Log("Picked up MetaCoin! New total: " + GameSession.Instance.metaCoins);

            Destroy(gameObject);
        }
    }
}
