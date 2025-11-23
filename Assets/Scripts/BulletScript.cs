using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private int damage;
    private float speed;

    public void Initialize(int damage, float speed, Vector2 direction)
    {
        rb = GetComponent<Rigidbody2D>();
        this.damage = damage;
        this.speed = speed;
        rb.linearVelocity = direction * speed;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyScript>().DamageEnemy(damage);
            Destroy(gameObject);
        }
    }
}
