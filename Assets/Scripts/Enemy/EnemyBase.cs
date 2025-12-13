using System.Collections;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    /* =========================
     * TUNING / DATA
     * ========================= */
    [Header("Stats")]
    [SerializeField] protected float maxHealth = 10f;
    [SerializeField] protected float patrolSpeed = 3f;
    [SerializeField] protected float aggroSpeed = 5f;
    [SerializeField] protected float knockbackDuration = 0.25f;
    [SerializeField] protected float minDifficulty;
    [SerializeField] protected float maxDifficulty;

    [Header("Patrol")]
    [SerializeField] protected float patrolRadius = 2.5f;
    [SerializeField] protected float patrolChangeTime = 1.5f;
    [SerializeField] protected float arriveDistance = 0.2f;
    [SerializeField] protected LayerMask obstacleMask;

    [Header("Aggro")]
    [SerializeField] protected float aggroPauseTime = 0.5f;
    [SerializeField] protected GameObject alertIcon;

    /* =========================
     * STATE
     * ========================= */
    public enum EnemyState { Patrol, Aggressive }
    public EnemyState CurrentState { get; protected set; } = EnemyState.Patrol;

    protected Rigidbody2D rb;
    protected PlayerController currentTargetPlayer;
    protected Vector2 lastMovementDirection;

    protected float currentHealth;
    protected float knockbackTimer;
    protected bool isPaused;

    Vector2 patrolTarget;
    float patrolTimer;

    public event System.Action<EnemyBase> OnDeath;

    /* =========================
     * UNITY LIFECYCLE
     * ========================= */
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        alertIcon = GetComponentInChildren<AlertIconMarker>(true)?.gameObject;
    }

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        PickNewPatrolTarget();
    }

    protected virtual void Update()
    {
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
            case EnemyState.Patrol:
                Patrol();
                break;

            case EnemyState.Aggressive:
                Aggro();
                break;
        }
    }

    protected virtual void Patrol()
    {
        patrolTimer -= Time.deltaTime;

        if (patrolTimer <= 0f ||
            Vector2.Distance(transform.position, patrolTarget) < arriveDistance ||
            Physics2D.OverlapBox(patrolTarget, Vector2.one, 0, obstacleMask))
        {
            PickNewPatrolTarget();
        }

        MoveTowards(patrolTarget, patrolSpeed);
    }

    protected virtual void Aggro()
    {
        if (currentTargetPlayer == null)
            return;

        Vector2 targetPos = currentTargetPlayer.transform.position;
        Vector2 dir = GetAggroDirection(targetPos);

        Move(dir, aggroSpeed);
        lastMovementDirection = dir;
    }

    /* =========================
     * MOVEMENT (SINGLE PATH)
     * ========================= */
    protected void Move(Vector2 direction, float speed)
    {
        if (isPaused)
        {
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

    protected void MoveTowards(Vector2 target, float speed)
    {
        Vector2 dir = (target - (Vector2)transform.position).normalized;
        lastMovementDirection = dir;
        Move(dir, speed);
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
        if (CurrentState == EnemyState.Patrol)
        {
            currentTargetPlayer = player;
            SwitchState(EnemyState.Aggressive);
        }

        if (isBullet)
            knockbackTimer = knockbackDuration;

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
        if (newState == EnemyState.Aggressive &&
            CurrentState != EnemyState.Aggressive)
        {
            StartCoroutine(AggroPause());
        }

        CurrentState = newState;
    }
    public void Alert(PlayerController player)
    {
        if (CurrentState == EnemyState.Patrol)
        {
            currentTargetPlayer = player;
            SwitchState(EnemyState.Aggressive);
        }
    }


    IEnumerator AggroPause()
    {
        isPaused = true;
        rb.linearVelocity = Vector2.zero;

        if (alertIcon != null)
            alertIcon.SetActive(true);

        yield return new WaitForSeconds(aggroPauseTime);

        if (alertIcon != null)
            alertIcon.SetActive(false);

        isPaused = false;
    }

    /* =========================
     * HELPERS
     * ========================= */
    void PickNewPatrolTarget()
    {
        patrolTimer = patrolChangeTime;
        patrolTarget = (Vector2)transform.position +
                       Random.insideUnitCircle * patrolRadius;
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

    public bool IsValidForDifficulty(int difficulty)
    {
        return difficulty >= minDifficulty && difficulty <= maxDifficulty;
    }

}
