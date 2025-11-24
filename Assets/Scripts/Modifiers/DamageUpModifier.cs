using UnityEngine;

[CreateAssetMenu(menuName = "Modifiers/Damage Up")]
public class DamageUpModifier : AttackModifier
{
    public float flatDamage = 20f;

    public override void ApplyAttack(ref AttackData data)
    {
        data.damage += flatDamage;
    }
}
