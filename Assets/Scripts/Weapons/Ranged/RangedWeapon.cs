using UnityEngine;

using System.Collections.Generic;


public class RangedWeapon : WeaponBase
{

    public override void Attack()
    {
        if (!canShoot) return;
        canShoot = false;

        // Richtungslogik bleibt 100% wie bei dir!
        Vector3 direction;

        if (usingController)
        {
            if (aimInput.sqrMagnitude < 0.1f)
                return;

            direction = new Vector3(aimInput.x, 0f, aimInput.y);
        }
        else
        {
            direction = GetMouseWorldPos() - attackPoint.position;
            direction.y = 0f;
        }


        if (direction == Vector3.zero)
            return;

        if (direction.sqrMagnitude < 0.22f)
            return;

        direction.Normalize();

        // AttackData erstellen
        AttackData atk = new AttackData();
        atk.sourcePlayer = owner;
        // Base weapon values
        atk.baseDamage = data.damage;
        atk.speed = data.bulletSpeed;
        //bullet Sprite aus weapon data
        atk.bulletSprite = data.bulletSprite;
        atk.bulletColor = data.bulletColor;

        // Initialize pipeline fields
        atk.flatAdd = 0f;
        atk.percentAdd = 0f;
        atk.multiplier = 1f;

        atk.critChance = 0f;
        atk.isCrit = false;
        atk.critMultiplier = 2f;

        // Bullet properties
        atk.size = 1f;
        atk.knockback = 0f;
        atk.pierce = 0;
        atk.bounce = 0;
        atk.knockback = 5;
        atk.isBullet = true;
        atk.range = data.range;
        atk.forwardDirection = direction;
        // Modifiers anwenden
        PlayerModifierManager modifierManager = GetComponentInParent<PlayerModifierManager>();
        modifierManager?.ApplyAttackModifiers(ref atk);
        if (!atk.isCrit)
        {
            if (Random.value <= atk.critChance)
            {
                atk.isCrit = true;
            }
        }

        // rotation for bullet direction
        Quaternion rot = Quaternion.LookRotation(direction);


        // spawn position with small offset
        Vector3 spawnPos = attackPoint.position + direction * 0.1f;
        spawnPos.y = attackPoint.position.y; // keep bullet floating at weapon height


        // Bullet Spawn + Initialize
        GameObject bullet = BulletPool.Instance.GetBullet();
        bullet.transform.position = spawnPos;
        bullet.transform.rotation = rot;
        List<AttackModifier> attackModifiers = modifierManager != null ? modifierManager.GetAttackModifiers() : new List<AttackModifier>();
        bullet.SetActive(true); // ENABLE LAST
        bullet.GetComponent<BulletScript>().Initialize(atk, attackModifiers);

    }
}
