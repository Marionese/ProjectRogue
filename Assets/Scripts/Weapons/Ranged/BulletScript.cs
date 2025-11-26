using UnityEngine;
using System.Collections.Generic;

public class BulletScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private AttackData data;  // <--- wir speichern das ganze AttackData
    private List<AttackModifier> attackModifiers = new();

    public void Initialize(AttackData data, List<AttackModifier> attackModifiers)
    {
        this.data = data;
        this.attackModifiers = attackModifiers != null ? new List<AttackModifier>(attackModifiers) : new List<AttackModifier>();
        rb = GetComponent<Rigidbody2D>();

        // Geschwindigkeit und Richtung wie vorher!
        rb.linearVelocity = data.direction * data.speed;
        Debug.Log("Bullet Damage: " + data.damage);
        // Rotation bleibt exakt gleich!
        float angle = Mathf.Atan2(data.direction.y, data.direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // Schadenssystem bleibt identisch
            EnemyScript enemy = collision.GetComponent<EnemyScript>();

            if (enemy != null)
            {
                enemy.DamageEnemy(data.damage);

                foreach (var mod in attackModifiers)
                {
                    mod.OnHit(enemy, data);
                }
            }

            // später hier: bounce, pierce, poison
            Destroy(gameObject);
        }
        else if ((collision.CompareTag("Ground") || collision.CompareTag("Wall"))
                  && !collision.gameObject.GetComponent<Plattform>())
        {
            // Mauern und Boden zerstören Bullet wie vorher
            Destroy(gameObject);
        }
    }
}
