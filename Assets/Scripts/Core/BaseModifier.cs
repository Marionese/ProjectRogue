using UnityEngine;

public abstract class BaseModifier : ScriptableObject {}

public abstract class AttackModifier : BaseModifier {
    public abstract void ApplyAttack(ref AttackData data);
    public virtual void OnHit(EnemyScript enemy, AttackData data) { }
}

public abstract class PlayerStatModifier : BaseModifier {
    public abstract void ApplyStats(PlayerRuntimeStats stats);
}
