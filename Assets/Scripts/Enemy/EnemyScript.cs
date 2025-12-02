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
    }

    //Helper Funktions
    public void DamageEnemy(float amount)
    {
        knockbackTime = 0.15f;
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
        rb.linearVelocity = dir * moveSpeed;
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
        Destroy(gameObject);
    }

}