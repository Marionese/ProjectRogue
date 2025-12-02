using UnityEngine;

[CreateAssetMenu(menuName = "Modifiers/KnockBack Up")]
public class KnockBackOnHit : AttackModifier
{
    public float force;
    public override void ApplyAttack(ref AttackData data)
    {
        data.knockback += force;
    }
    public override void OnHit(EnemyScript enemy, AttackData data)
    {
      
    }
}