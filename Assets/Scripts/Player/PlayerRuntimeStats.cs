using UnityEngine;

[System.Serializable]
public class PlayerRuntimeStats
{
    public float moveSpeed;
    public int maxHP;
    public int currentHP;

    // Nested snapshot struct
    [System.Serializable]
    public struct Snapshot
    {
        public int hp;
    }

    public Snapshot CreateSnapshot()
    {
        return new Snapshot
        {
            hp = currentHP
        };
    }

    public void ApplySnapshot(Snapshot snap)
    {
        // Wenn noch nie ein Snapshot gespeichert wurde, bleibt hp = 0
        // Dann lassen wir currentHP einfach wie sie ist (maxHP)
        if (snap.hp > 0)
        {
            currentHP = Mathf.Min(snap.hp, maxHP);
        }
        else
        {
            currentHP = maxHP; // z.B. am Anfang des Runs
        }
    }

    public PlayerRuntimeStats(PlayerBaseStats baseStats)
    {
        moveSpeed = baseStats.moveSpeed;
        maxHP = baseStats.maxHP;
        currentHP = maxHP;

    }
}
