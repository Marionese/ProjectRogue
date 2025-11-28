using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;


public class RangedWeapon : WeaponBase
{
    private bool usingController;
    private bool allowMouse;
    private Camera cam;
    private bool canShoot = true;
    private float timer;
    private Vector2 aimInput;
    void Start()
    {

        cam = Camera.main;
        var input = GetComponentInParent<PlayerInput>();
        allowMouse = input != null && input.currentControlScheme == "Keyboard&Mouse";
    }
    //inputs
    public override void SetAim(Vector2 input)
    {
        aimInput = input;

        if (input.sqrMagnitude > 0.1f)
            usingController = true;
    }
    void Update()
    {
        RotateWeapon();
        HandleCooldown();
    }

    void HandleCooldown()
    {
        if (!canShoot)
        {
            timer += Time.deltaTime;
            if (timer >= data.fireRate)
            {
                canShoot = true;
                timer = 0;
            }
        }
    }

    public override void Attack()
    {
        if (!canShoot) return;
        canShoot = false;

        // Richtungslogik bleibt 100% wie bei dir!
        Vector2 direction;

        if (usingController)
        {
            if (aimInput.sqrMagnitude > 0.1f)
                direction = aimInput;         // Stick aktiv
            else
                direction = transform.right;  // letzte Waffenrotation verwenden
        }
        else
        {
            direction = allowMouse ? (GetMouseWorldPos() - attackPoint.position) : Vector2.zero;
        }

        if (direction == Vector2.zero)
            return;

        if (direction.sqrMagnitude < 0.22f)
            return;

        direction.Normalize();

        // AttackData erstellen
        AttackData atk = new AttackData();

        // Base weapon values
        atk.baseDamage = data.damage;
        atk.speed = data.bulletSpeed;

        // Initialize pipeline fields
        atk.flatAdd = 0f;
        atk.percentAdd = 0f;
        atk.multiplier = 1f;

        atk.critChance = 0.1f; //10% critChance test
        atk.isCrit = false;
        atk.critMultiplier = 2f;

        // Bullet properties
        atk.size = 0.2f;
        atk.knockback = 0f;
        atk.pierce = 0;
        atk.bounce = 0;

        // Modifiers anwenden
        PlayerModifierManager modifierManager = GetComponentInParent<PlayerModifierManager>();
        modifierManager?.ApplyAttack(ref atk);
        if (!atk.isCrit)
        {
            if (Random.value <= atk.critChance)
            {
                atk.isCrit = true;
            }
        }

        // rotation for bullet direction
        Quaternion rot = Quaternion.FromToRotation(Vector3.right, direction);

        // spawn position with small offset
        Vector3 spawnPos = attackPoint.position + (Vector3)(direction * 0.1f);

        // Bullet Spawn + Initialize
        GameObject bullet = Instantiate(data.bulletPrefab, spawnPos, rot);
        List<AttackModifier> attackModifiers = modifierManager != null ? modifierManager.GetAttackModifiers() : new List<AttackModifier>();
        bullet.GetComponent<BulletScript>().Initialize(atk, attackModifiers);

    }


    void RotateWeapon()
    {


        if (aimInput.sqrMagnitude > 0.1f)
        {
            usingController = true;
            float stickAngle = Mathf.Atan2(aimInput.y, aimInput.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, stickAngle);
            return;
        }
        var mouse = Mouse.current;
        if (allowMouse && mouse != null && mouse.delta.ReadValue().sqrMagnitude > 2f) // threshold
        {
            usingController = false;
        }
        if (usingController || !allowMouse)
        {
            return; // keep last controller rotation!
        }
        Vector2 dir = GetMouseWorldPos() - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    Vector3 GetMouseWorldPos()
    {
        return cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }
}
