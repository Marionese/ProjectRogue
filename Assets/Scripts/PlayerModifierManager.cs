using System.Collections.Generic;
using UnityEngine;

public class PlayerModifierManager : MonoBehaviour
{
    private List<AttackModifier> activeMods = new();

    public void AddItem(ItemData item)
    {
        foreach (var m in item.modifiers)
            activeMods.Add(m);
    }

    public void RemoveItem(ItemData item)
    {
        foreach (var m in item.modifiers)
            activeMods.Remove(m);
    }

    public void Apply(ref AttackData data)
    {
        foreach (var m in activeMods)
            m.Apply(ref data);
    }
}
