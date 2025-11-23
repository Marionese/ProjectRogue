using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public WeaponData data;           // ScriptableObject Daten
    public Transform attackPoint;     // BulletSpawn / Slash-Point

    // Jedes Weapon-Kind muss Attack() haben:
    public abstract void Attack();
}
