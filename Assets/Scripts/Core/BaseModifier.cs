using UnityEngine;

public abstract class BaseModifier : ScriptableObject
{
    [Tooltip("Lower = applied earlier. Example: Flat bonuses (0), then percent (1), then multipliers (2).")]
    public int priority = 0;
}

public abstract class AttackModifier : BaseModifier
{
    public abstract void ApplyAttack(ref AttackData data);
    public virtual void OnHit(EnemyScript enemy, AttackData data) { }
}

public abstract class PlayerStatModifier : BaseModifier
{
    public abstract void ApplyStats(PlayerRuntimeStats stats);
}
