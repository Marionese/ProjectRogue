using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Rendering.Analytics;
using System.Data.Common;

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

        //set Bullet Sprite
        var sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            if (data.bulletSprite != null)
                sr.sprite = data.bulletSprite;

            sr.color = data.bulletColor;
        }


    }
    void Update()
    {
        if (lifetime <= 0)
        {
            data.hitPoint = transform.position;
            foreach (var mod in attackModifiers)
                mod.OnHitEnvironment(data);
            BulletPool.Instance.ReturnBullet(gameObject);
        }
        else
        {
            lifetime -= Time.deltaTime;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector2 surfaceHit = collision.ClosestPoint(transform.position);
        data.hitPoint = surfaceHit;
        if (collision.CompareTag("EnemyCollider"))
        {
            if (data.sourcePlayer == null)
                return;
            EnemyBase enemy = collision.GetComponentInParent<EnemyBase>();
            if (enemy != null)
            {
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
                enemy.DamageEnemy(dmg, data.isBullet, data.sourcePlayer);
                ApplyKnockback(enemy, data.forwardDirection);

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
            foreach (var mod in attackModifiers)
                mod.OnHitEnvironment(data);
            BulletPool.Instance.ReturnBullet(gameObject);

        }
        else if (collision.CompareTag("PlayerHitBox"))
        {
            if (data.sourcePlayer != null)
                return;

            var player = collision.GetComponentInParent<PlayerController>();
            int damage = Mathf.Max(1, Mathf.RoundToInt(data.baseDamage));
            player.DamagePlayer(damage);
            BulletPool.Instance.ReturnBullet(gameObject);
        }
    }
    void ApplyKnockback(EnemyBase enemy, Vector2 dir)
    {
        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        // Richtung: vom Bullet zum Enemy (Push-Away)



        rb.linearVelocity += dir * data.knockback;
    }
}
