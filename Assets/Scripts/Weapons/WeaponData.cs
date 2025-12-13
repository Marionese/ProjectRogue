using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public float fireRate = 0.2f;

    public int damage = 1;
    public float bulletSpeed = 10f;
    public float range=1;
    public GameObject bulletPrefab;
    public Sprite bulletSprite;
    public Color bulletColor;
}
