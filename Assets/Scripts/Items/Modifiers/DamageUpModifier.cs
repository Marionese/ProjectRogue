using UnityEngine;

[CreateAssetMenu(menuName = "Modifiers/Damage Flat Up")]
public class DamageUpModifier : AttackModifier
{
    public float flatDamage;

    public override void ApplyAttack(ref AttackData data)
    {
        data.flatAdd += flatDamage;
    }
}
