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
    public enum EnemyState { Patrol, Aggressive, Attacking }
    public EnemyState CurrentState { get; protected set; } = EnemyState.Patrol;

    protected Rigidbody rb;
    protected PlayerController currentTargetPlayer;
    protected Vector3 lastMovementDirection;

    protected float currentHealth;
    protected float knockbackTimer;
    protected bool isPaused;
    protected Coroutine attackCoroutine;

    Vector3 patrolTarget;
    float patrolTimer;

    public event System.Action<EnemyBase> OnDeath;

    /* =========================
     * UNITY LIFECYCLE
     * ========================= */
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;

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

            case EnemyState.Attacking:
                if (attackCoroutine == null)
                    attackCoroutine = StartCoroutine(Attack());
                break;
        }
    }

    protected virtual void Patrol()
    {
        patrolTimer -= Time.deltaTime;

        if (patrolTimer <= 0f ||
            Vector3.Distance(transform.position, patrolTarget) < arriveDistance ||
            Physics.OverlapBox(
                patrolTarget,
                Vector3.one * 0.4f,
                Quaternion.identity,
                obstacleMask
            ).Length > 0)
        {
            PickNewPatrolTarget();
        }

        MoveTowards(patrolTarget, patrolSpeed);
    }

    protected virtual void Aggro()
    {
        if (currentTargetPlayer == null)
            return;

        Vector3 targetPos = currentTargetPlayer.transform.position;
        Vector3 dir = GetAggroDirection(targetPos);

        Move(dir, aggroSpeed);
        lastMovementDirection = dir;
    }

    protected virtual IEnumerator Attack()
    {
        yield break;
    }

    /* =========================
     * MOVEMENT (SINGLE PATH)
     * ========================= */
    protected void Move(Vector3 direction, float speed)
    {
        if (isPaused)
        {
            if (attackCoroutine == null)
                rb.linearVelocity = Vector3.zero;
            return;
        }

        if (knockbackTimer > 0f)
        {
            knockbackTimer -= Time.deltaTime;
            return;
        }

        direction.y = 0f;
        direction.Normalize();

        Vector3 targetVelocity = direction * speed;
        Vector3 current = rb.linearVelocity;

        Vector3 desired = new Vector3(
            targetVelocity.x,
            current.y,        // 🔒 preserve Y
            targetVelocity.z
        );

        rb.linearVelocity = Vector3.Lerp(current, desired, 0.2f);

    }

    protected void MoveTowards(Vector3 target, float speed)
    {
        Vector3 dir = target - transform.position;
        dir.y = 0f;
        dir.Normalize();

        lastMovementDirection = dir;
        Move(dir, speed);
    }

    protected virtual Vector3 GetAggroDirection(Vector3 targetPos)
    {
        FlowField field = null;

        if (currentTargetPlayer.PlayerID == 0)
            field = FlowFieldManager.Instance.flowFieldP1;
        else if (currentTargetPlayer.PlayerID == 1)
            field = FlowFieldManager.Instance.flowFieldP2;

        Vector3 flowDir = field != null
            ? field.GetFlowDirection(transform.position)
            : Vector3.zero;

        if (flowDir == Vector3.zero)
            flowDir = (targetPos - transform.position).normalized;

        flowDir += Random.insideUnitSphere * 0.1f;
        flowDir.y = 0f;

        return flowDir.normalized;
    }

    /* =========================
     * DAMAGE / DEATH
     * ========================= */
    public virtual void DamageEnemy(float amount, bool isBullet, PlayerController player)
    {
        Alert(player);

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
        CurrentState = newState;
    }

    public void Alert(PlayerController player)
    {
        if (CurrentState == EnemyState.Patrol)
        {
            currentTargetPlayer = player;
            StartCoroutine(AggroPause());
            SwitchState(EnemyState.Aggressive);
        }
    }

    public void PlayerGotInRange(PlayerController player)
    {
        if (currentTargetPlayer != player)
            return;

        if (CurrentState == EnemyState.Aggressive)
            SwitchState(EnemyState.Attacking);
    }

    public void LeftPlayerRange(PlayerController player)
    {
        if (currentTargetPlayer != player)
            return;

        if (CurrentState == EnemyState.Attacking)
            SwitchState(EnemyState.Aggressive);
    }

    IEnumerator AggroPause()
    {
        isPaused = true;
        rb.linearVelocity = Vector3.zero;

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

        Vector2 rnd = Random.insideUnitCircle * patrolRadius;
        patrolTarget = transform.position + new Vector3(rnd.x, 0f, rnd.y);
    }

    protected virtual void UpdateFacingDirection()
    {
        if (knockbackTimer > 0f)
            return;

        float scale = Mathf.Abs(transform.localScale.x);

        if (lastMovementDirection.x > 0.1f)
            transform.localScale = new Vector3(scale, scale, scale);
        else if (lastMovementDirection.x < -0.1f)
            transform.localScale = new Vector3(-scale, scale, scale);
    }

    public bool IsValidForDifficulty(int difficulty)
    {
        return difficulty >= minDifficulty && difficulty <= maxDifficulty;
    }
}
