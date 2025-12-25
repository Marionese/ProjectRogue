using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    /* =========================
     * TUNING / DATA
     * ========================= */
    [Header("Stats")]
    [SerializeField] protected float maxHealth = 10f;
    [SerializeField] protected float movementSpeed = 5f;
    [SerializeField] protected float knockbackDuration = 0.1f;
    [SerializeField] protected float minDifficulty;
    [SerializeField] protected float maxDifficulty;

    /* =========================
     * STATE
     * ========================= */
    public enum EnemyState {Aggressive, Attacking }
    public EnemyState CurrentState { get; protected set; } = EnemyState.Aggressive;
    protected Rigidbody2D rb;
    protected PlayerController currentTargetPlayer;
    protected Vector2 lastMovementDirection;

    protected float currentHealth;
    protected float knockbackTimer;
    protected bool isPaused;
    protected Coroutine attackCoroutine;

    Vector2 patrolTarget;
    float patrolTimer;

    public event System.Action<EnemyBase> OnDeath;

    /* =========================
     * UNITY LIFECYCLE
     * ========================= */
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    protected virtual void Update()
    {
        PickClosestPlayer();
        UpdateState();
        UpdateFacingDirection();
    }

    /* =========================
     * STATE LOGIC (OVERRIDABLE)
     * ========================= */
    protected virtual void UpdateState()
    {
        switch (CurrentState)
        {
            case EnemyState.Aggressive:
                Aggro();
                break;
            case EnemyState.Attacking:
                if (attackCoroutine == null)
                    attackCoroutine = StartCoroutine(Attack());
                break;
        }
    }

    protected virtual void Aggro()
    {
        if (currentTargetPlayer == null)
            return;

        Vector2 targetPos = currentTargetPlayer.transform.position;
        Vector2 dir = GetAggroDirection(targetPos);

        Move(dir, movementSpeed);
        lastMovementDirection = dir;
    }
    protected virtual IEnumerator Attack()
    {
        yield break;
    }
    /* =========================
     * MOVEMENT (SINGLE PATH)
     * ========================= */
    protected void Move(Vector2 direction, float speed)
    {
        if (isPaused)
        {
            if (attackCoroutine == null)
                rb.linearVelocity = Vector2.zero;
            return;
        }

        if (knockbackTimer > 0f)
        {
            knockbackTimer -= Time.deltaTime;
            return;
        }

        Vector2 targetVelocity = direction.normalized * speed;
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, targetVelocity, 0.2f);
    }
    protected virtual Vector2 GetAggroDirection(Vector2 targetPos)
    {

        FlowField field = null;

        if (currentTargetPlayer.PlayerID == 0)
            field = FlowFieldManager.Instance.flowFieldP1;
        else if (currentTargetPlayer.PlayerID == 1)
            field = FlowFieldManager.Instance.flowFieldP2;

        Vector2 flowDir = field != null
            ? field.GetFlowDirection(transform.position)
            : Vector2.zero;

        if (flowDir == Vector2.zero)
            flowDir = (targetPos - (Vector2)transform.position).normalized;

        flowDir += Random.insideUnitCircle * 0.1f;
        return flowDir.normalized;
    }

    /* =========================
     * DAMAGE / DEATH
     * ========================= */
    public virtual void DamageEnemy(float amount, bool isBullet, PlayerController player)
    {
        currentHealth -= amount;

        if (currentHealth <= 0f)
            Die();
    }

    protected virtual void Die()
    {
        OnDeath?.Invoke(this);
        Destroy(gameObject);
    }

    /* =========================
     * STATE TRANSITIONS
     * ========================= */
    protected void SwitchState(EnemyState newState)
    {
        CurrentState = newState;
    }
    public void PlayerGotInRange(PlayerController player)
    {
        currentTargetPlayer = player;
        SwitchState(EnemyState.Attacking);
    }
    public void LeftPlayerRange(PlayerController player)
    {
        if (currentTargetPlayer != player)
            return;
        if (CurrentState == EnemyState.Attacking)
        {
            SwitchState(EnemyState.Aggressive);
        }
    }
    /* =========================
     * HELPERS
     * ========================= */

    void PickClosestPlayer()
    {
        var players = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);
        var amount = players.Count<PlayerController>();
        if (amount == 1)
        {
            currentTargetPlayer = players[0];
            return;
        }
        var player1_pos = players[0].transform.position;
        var player2_pos = players[1].transform.position;
        if (Vector3.Distance(transform.position, player1_pos) >= Vector3.Distance(transform.position, player2_pos))
        {
            currentTargetPlayer = players[0];
        }
        else
        {
            currentTargetPlayer = players[1];
        }

    }
    protected virtual void UpdateFacingDirection()
    {
        if (knockbackTimer > 0f)
            return;

        float scale = Mathf.Abs(transform.localScale.x);

        if (lastMovementDirection.x > 0.1f)
            transform.localScale = new Vector3(scale, scale, 1);
        else if (lastMovementDirection.x < -0.1f)
            transform.localScale = new Vector3(-scale, scale, 1);
    }
    public virtual void KnockBackEnemy(float force, Vector2 direction)
    {
        direction.Normalize();
        rb.linearVelocity = Vector2.zero;
        rb.linearVelocity = direction * force;
        knockbackTimer = knockbackDuration;
    }

    public bool IsValidForDifficulty(int difficulty)
    {
        return difficulty >= minDifficulty && difficulty <= maxDifficulty;
    }

}
