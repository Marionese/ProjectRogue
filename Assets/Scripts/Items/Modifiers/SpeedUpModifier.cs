using UnityEngine;

[CreateAssetMenu(menuName = "Modifiers/Speed Up")]
public class SpeedUpModifier : PlayerStatModifier
{
    public float speedup = 20f;

    public override void ApplyStats(PlayerRuntimeStats stats)
    {
        stats.moveSpeed += speedup;
    }
}
