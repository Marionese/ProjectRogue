using UnityEngine;

public class HealthPickup : BasePickup
{
    public int amount = 1;

    protected override void OnPickup(GameObject player)
    {
        player.GetComponent<PlayerController>().HealPlayer(amount);
    }
}
