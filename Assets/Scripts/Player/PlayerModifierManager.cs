using System.Collections.Generic;
using UnityEngine;

public class PlayerModifierManager : MonoBehaviour
{
    private List<BaseModifier> activeItems = new();

    // When an item is picked up
    public void AddItem(ItemData item)
    {
        activeItems.AddRange(item.modifiers);

        // Apply permanent stat bonuses
        foreach (var m in item.modifiers)
        {
            if (m is PlayerStatModifier stat)
                stat.ApplyStats(GetComponent<PlayerMovement>().RuntimeStats);
        }
    }

    // When an item is removed
    public void RemoveItem(ItemData item)
    {
        foreach (var m in item.modifiers)
        {
            activeItems.Remove(m);
        }

        // OPTIONAL: Undo stat modifiers (only if you want removable items)
    }

    // Called by weapons when firing
    public void ApplyAttack(ref AttackData data)
    {
        foreach (var m in activeItems)
        {
            if (m is AttackModifier atk)
                atk.ApplyAttack(ref data);
        }
    }

    public List<AttackModifier> GetAttackModifiers()
    {
        List<AttackModifier> modifiers = new();
        foreach (var m in activeItems)
        {
            if (m is AttackModifier atk)
                modifiers.Add(atk);
        }

        return modifiers;
    }
}
