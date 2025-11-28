using System.Collections.Generic;
using UnityEngine;

public class PlayerModifierManager : MonoBehaviour
{
    private List<BaseModifier> activeItems = new();

    public void AddItem(ItemData item)
    {
        activeItems.AddRange(item.modifiers);

        var controller = GetComponent<PlayerController>();

        foreach (var m in item.modifiers)
        {
            if (m is PlayerStatModifier stat)
                stat.ApplyStats(controller.runtimeStats);
        }
    }

    public void RemoveItem(ItemData item)
    {
        foreach (var m in item.modifiers)
            activeItems.Remove(m);
    }

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

    // Use PlayerID from the controller, not external playerIndex
    public void InitializeFromSession()
    {
        activeItems.Clear();

        var controller = GetComponent<PlayerController>();
        int playerID = controller.PlayerID;

        // 1. Reset runtime stats to baseStats
        controller.ResetRuntimeStats();

        // 2. Restore snapshot (HP, temp buffs)
        var snapshot = GameSession.Instance.GetPlayerSnapshot(playerID);
        controller.runtimeStats.ApplySnapshot(snapshot);

        // 3. Restore permanent item upgrades ONCE
        var items = (playerID == 0)
            ? GameSession.Instance.player1RunItems
            : GameSession.Instance.player2RunItems;

        foreach (var item in items)
        {
            activeItems.AddRange(item.modifiers);

            foreach (var m in item.modifiers)
            {
                if (m is PlayerStatModifier stat)
                    stat.ApplyStats(controller.runtimeStats);
            }
        }

        Debug.Log($"Player {playerID} initialized from session with {activeItems.Count} modifiers.");
    }
}
