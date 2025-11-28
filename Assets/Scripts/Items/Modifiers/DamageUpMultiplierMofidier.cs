using UnityEngine;

[CreateAssetMenu(menuName = "Modifiers/Damage 2x")]
public class DamageUpMultiplierMofidier : AttackModifier
{
    public float multiplier = 2f;

    public override void ApplyAttack(ref AttackData data)
    {
        data.damage *= 2;
    }
}
