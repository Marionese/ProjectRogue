using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float maxHealth;
    public PlayerController currentTargetPlayer;
    private Rigidbody2D rb;
    private float knockbackTime;
    private float moveSpeed = 5;
    float currentHealt;
    private Vector2 playerPos;
    public enum EnemyState { patrol, aggressive }
    private EnemyState currentState = EnemyState.patrol;
    public event System.Action<EnemyScript> OnDeath;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealt = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case EnemyState.patrol:
                break;

            case EnemyState.aggressive:
                AggroPlayer();
                break;
        }
        UpdateFacingDirection();
    }

    //Helper Funktions
    public void DamageEnemy(float amount, bool isBullet)
    {
        if (isBullet)
            knockbackTime = 0.25f;
        currentHealt -= amount;
        if (currentHealt <= 0)
        {
            Die();
        }
    }
    void Patrol()
    {
        return;
    }
    void AggroPlayer()
    {
        playerPos = currentTargetPlayer.transform.position;
        MoveTo(playerPos);
    }
    void MoveTo(Vector2 pos)
    {
        if (knockbackTime > 0)
        {
            knockbackTime -= Time.deltaTime;
            return; // skip AI movement, let physics play out
        }

        Vector2 dir = (pos - (Vector2)transform.position).normalized;
        Vector2 targetVel = dir * moveSpeed;
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, targetVel, 0.3f);
    }
    public void SwitchState(EnemyState state)
    {
        currentState = state;
    }
    public void SetTarget(PlayerController player)
    {
        currentTargetPlayer = player;
    }
    void Die()
    {
        OnDeath?.Invoke(this);
        Destroy(gameObject);
    }
    void UpdateFacingDirection()
    {
        Vector2 vel = rb.linearVelocity;

        float scale = Mathf.Abs(transform.localScale.x); // keep original size

        if (vel.x > 0.1f)
            transform.localScale = new Vector3(scale, scale, 1);
        else if (vel.x < -0.1f)
            transform.localScale = new Vector3(-scale, scale, 1);
    }
}