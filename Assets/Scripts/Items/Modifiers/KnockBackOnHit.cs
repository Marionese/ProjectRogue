using UnityEngine;

[CreateAssetMenu(menuName = "Modifiers/KnockBack")]
public class KnockBackOnHit : AttackModifier
{
    public float force = 5f;
    public override void ApplyAttack(ref AttackData data)
    {

    }
    public override void OnHit(EnemyScript enemy, AttackData data)
    {
        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity += data.direction * force;
        }
    }
}
