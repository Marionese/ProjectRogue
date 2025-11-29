using UnityEngine;

[RequireComponent(typeof(Health))]
public class EnemyScript : MonoBehaviour
{
    public Vector2 BulletHitPosition { get; set; }

    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        health.OnDied += HandleDeath;
    }

    private void OnDisable()
    {
        health.OnDied -= HandleDeath;
    }

    // API für Bullets etc.
    public void DamageEnemy(float amount)
    {
        health.TakeDamage(amount);
    }

    private void HandleDeath()
    {
        Destroy(gameObject);
    }
}
