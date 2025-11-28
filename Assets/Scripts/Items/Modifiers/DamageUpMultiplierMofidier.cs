using UnityEngine;

[CreateAssetMenu(menuName = "Modifiers/Damage Multiplier")]
public class DamageUpMultiplierMofidier : AttackModifier
{
    public float multiplier;

    public override void ApplyAttack(ref AttackData data)
    {
        data.multiplier *= multiplier;
    }
}
