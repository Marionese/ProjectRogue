using UnityEngine;

public class MetaCoinPickup : BasePickup
{
    public int amount = 1;

    protected override void OnPickup(GameObject player)
    {
        GameSession.Instance.metaCoins += amount;
        SaveSystem.SaveGame(GameSession.Instance.currentSlot);

        Debug.Log($"Picked up {amount} MetaCoin(s)! Total now: {GameSession.Instance.metaCoins}");
    }
}
