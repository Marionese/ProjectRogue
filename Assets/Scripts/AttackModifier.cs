using UnityEngine;

public abstract class AttackModifier : ScriptableObject
{
    public abstract void Apply(ref AttackData data);
}
