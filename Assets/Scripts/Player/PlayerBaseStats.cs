using UnityEngine;

[CreateAssetMenu(menuName = "Player/Player Stats")]
public class PlayerBaseStats : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Max Hp")]
    public int maxHP = 6; //3 full hearts 6 hits 
    
}
