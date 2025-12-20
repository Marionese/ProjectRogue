using UnityEngine;

[System.Serializable]
public struct AttackData
{
    // DAMAGE PIPELINE
    public float baseDamage;

    public float flatAdd;
    public float percentAdd;
    public float multiplier;

    public bool isCrit;
    public float critMultiplier;
    public float critChance;
    
    // BULLET PROPERTIES
    public float speed;
    public float size;
    public float knockback;
    public int pierce;
    public int bounce;
    public float range;
    public bool isBullet;
    public Vector3 forwardDirection;
    public Vector3 hitPoint;
    public Sprite bulletSprite;
    public Color bulletColor;
    public PlayerController sourcePlayer;

}

