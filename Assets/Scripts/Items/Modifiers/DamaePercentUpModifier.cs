using UnityEngine;

[CreateAssetMenu(menuName = "Modifiers/Damage Percent Up")]
public class DamaePercentUpModifier : AttackModifier
{
    public float percent;

    public override void ApplyAttack(ref AttackData data)
    {
        data.percentAdd += percent;
    }
}
