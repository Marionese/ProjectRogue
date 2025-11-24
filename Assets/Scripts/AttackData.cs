using UnityEngine;

[System.Serializable]
public struct AttackData
{
    public float damage;
    public float speed;
    public float knockback;

    public int bounceCount;
    public int pierceCount;

    public bool appliesPoison;
    public float poisonChance;

    public Vector2 direction;
}
