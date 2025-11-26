using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/New Item")]
public class ItemData : ScriptableObject
{  
    public bool isUnique;
    public ItemCategory category;
    public string itemID;
    public string itemName;
    public Sprite icon;
    public List<BaseModifier> modifiers;
}
