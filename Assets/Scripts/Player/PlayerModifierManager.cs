using System.Collections.Generic;
using UnityEngine;

public class PlayerModifierManager : MonoBehaviour
{
    private List<BaseModifier> activeItems = new();

    public void AddItem(ItemData item)
    {
        activeItems.AddRange(item.modifiers);

        var controller = GetComponent<PlayerController>();
        var stats = controller.runtimeStats;

        foreach (var m in item.modifiers)
        {
            if (m is MaxHpModifier stat)
            {
                // MaxHP vor dem Modifier merken
                int beforeMax = stats.maxHP;

                stat.ApplyStats(stats);

                // MaxHP nach Modifier
                int afterMax = stats.maxHP;

                // Wenn MaxHP gestiegen ist → einmalig um die Differenz heilen
                int diff = afterMax - beforeMax;
                if (diff > 0)
                {
                    stats.currentHP += diff;
                    stats.currentHP = Mathf.Min(stats.currentHP, stats.maxHP);
                }
            }
        }
    }

    public void RemoveItem(ItemData item)
    {
        foreach (var m in item.modifiers)
            activeItems.Remove(m);
    }

    public void ApplyAttackModifiers(ref AttackData data)
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
        var stats = controller.runtimeStats;
        int playerID = controller.PlayerID;

        // 1. Stats vollständig resetten (frische RuntimeStats erzeugen!)
        controller.ResetRuntimeStats();
        stats = controller.runtimeStats;

        // 2. Items aus Session holen
        var items = (playerID == 0)
            ? GameSession.Instance.player1RunItems
            : GameSession.Instance.player2RunItems;

        // 3. Modifiers EINMALIG anwenden (ohne Heilung)
        foreach (var item in items)
        {
            activeItems.AddRange(item.modifiers);

            foreach (var m in item.modifiers)
            {
                if (m is PlayerStatModifier stat)
                {
                    stat.ApplyStats(stats);
                }
            }
        }

        // 4. Snapshot anwenden – MIT SAFETY
        var snapshot = GameSession.Instance.GetPlayerSnapshot(playerID);

        if (snapshot.hp > 0)
        {
            stats.currentHP = Mathf.Min(snapshot.hp, stats.maxHP);
        }
        else
        {
            stats.currentHP = stats.maxHP; // Startvolle HP
        }

        Debug.Log(
            $"Player {playerID} init. HP {stats.currentHP}/{stats.maxHP}, " +
            $"Speed {stats.moveSpeed}, Items {activeItems.Count}");
    }

}
