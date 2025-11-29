using UnityEngine;

public class CoinPickup : BasePickup
{
    public int amount = 1;

    protected override void OnPickup(GameObject player)
    {
        GameSession.Instance.coins += amount;
        Debug.Log($"Picked up {amount} Coin(s)! Total now: {GameSession.Instance.coins}");
    }
}
