using UnityEngine;

[CreateAssetMenu(menuName = "Modifiers/CritChance Up")]
public class CritChanceModifier : AttackModifier
{
    public float extraCritChance; // 0.1f

    public override void ApplyAttack(ref AttackData data)
    {
        data.critChance += extraCritChance;
    }
}
