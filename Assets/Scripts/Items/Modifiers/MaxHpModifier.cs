using UnityEngine;

[CreateAssetMenu(menuName = "Modifiers/MaxHp Modifier")]
public class MaxHpModifier : PlayerStatModifier
{
    public int amount;

    public override void ApplyStats(PlayerRuntimeStats stats)
    {
        stats.maxHP += amount;
    }
}