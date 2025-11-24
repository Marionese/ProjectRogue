using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private AttackData data;  // <--- wir speichern das ganze AttackData

    public void Initialize(AttackData data)
    {
        this.data = data;
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
            collision.GetComponent<EnemyScript>().DamageEnemy(data.damage);

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
