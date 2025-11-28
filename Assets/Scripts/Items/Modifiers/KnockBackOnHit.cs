using UnityEngine;

[CreateAssetMenu(menuName = "Modifiers/KnockBack Up")]
public class KnockBackOnHit : AttackModifier
{
    public float force;
    public override void ApplyAttack(ref AttackData data)
    {

    }
    public override void OnHit(EnemyScript enemy, AttackData data)
    {
        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        // Richtung: vom Bullet zum Enemy (Push-Away)
        Vector2 dir = ((Vector2)enemy.transform.position - enemy.BulletHitPosition).normalized;


        rb.linearVelocity += dir * force;
    }
}