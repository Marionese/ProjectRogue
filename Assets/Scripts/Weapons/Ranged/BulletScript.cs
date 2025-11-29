using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Rendering.Analytics;

public class BulletScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private AttackData data;
    private List<AttackModifier> attackModifiers = new();
    private float lifetime;

    public void Initialize(AttackData data, List<AttackModifier> attackModifiers)
    {
        this.data = data;
        this.attackModifiers = attackModifiers != null ? new List<AttackModifier>(attackModifiers) : new List<AttackModifier>();

        rb = GetComponent<Rigidbody2D>();

        // Richtung kommt vom Transform (Rotation)
        rb.linearVelocity = transform.right * data.speed;

        // Skalierung aus AttackData
        transform.localScale = Vector3.one * data.size;
        lifetime = data.range;
    }
    void Update()
    {
        if (lifetime <= 0)
        {
            BulletPool.Instance.ReturnBullet(gameObject);
        }
        else
        {
            lifetime -= Time.deltaTime;
        }
    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyScript enemy = collision.GetComponent<EnemyScript>();
            if (enemy != null)
            {
                enemy.BulletHitPosition = transform.position;
                // DAMAGE PIPELINE FINAL CALCULATION
                float dmg = data.baseDamage;

                dmg += data.flatAdd;
                dmg *= (1 + data.percentAdd);
                dmg *= data.multiplier;
                if (data.isCrit)
                {
                    Debug.Log("CRIT!");
                    dmg *= data.critMultiplier;

                }

                // Mindestschaden 1
                dmg = Mathf.Max(1, dmg);

                // Schaden anwenden
                Debug.Log("Damage" + dmg);
                enemy.DamageEnemy(dmg);

                // ON-HIT MODIFIERS
                foreach (var mod in attackModifiers)
                {
                    mod.OnHit(enemy, data);
                }
            }

            BulletPool.Instance.ReturnBullet(gameObject);

        }
        else if ((collision.CompareTag("Ground") || collision.CompareTag("Wall"))
                 && !collision.gameObject.GetComponent<Plattform>())
        {
            BulletPool.Instance.ReturnBullet(gameObject);

        }
    }
}
