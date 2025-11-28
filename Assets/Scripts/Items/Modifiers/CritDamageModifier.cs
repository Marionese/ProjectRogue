using UnityEngine;

[CreateAssetMenu(menuName = "Modifiers/CritDamage Up")]
public class CritDamageModifier : AttackModifier
{
    public float extraMultiplier; // z. B. 1.5

    public override void ApplyAttack(ref AttackData data)
    {
        data.critMultiplier *= extraMultiplier;
    }
}
