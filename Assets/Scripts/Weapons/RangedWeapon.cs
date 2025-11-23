using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class RangedWeapon : WeaponBase
{
    private bool usingController;

    private Camera cam;
    private bool canShoot = true;
    private float timer;
    InputAction aimAction;
    void Start()
    {
        aimAction = InputSystem.actions.FindAction("Aim");
        cam = Camera.main;
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

        Vector2 stick = aimAction.ReadValue<Vector2>();
        Vector2 direction;

        if (stick.sqrMagnitude > 0.1f)         // controller aiming
        {
            usingController = true;
            direction = stick;
        }
        else                                   // mouse aiming
        {
            usingController = false;
            direction = (GetMouseWorldPos() - attackPoint.position);
        }

        if (direction.sqrMagnitude < 0.22f)
            return;

        direction.Normalize();

        Vector3 spawnPos = attackPoint.position + (Vector3)(direction * 0.1f);

        GameObject bullet = Instantiate(data.bulletPrefab, spawnPos, Quaternion.identity);
        bullet.GetComponent<BulletScript>().Initialize(data.damage, data.bulletSpeed, direction);
    }


    void RotateWeapon()
    {
        Vector2 stick = aimAction.ReadValue<Vector2>();

        if (stick.sqrMagnitude > 0.1f)
        {
            usingController = true;
            float stickAngle = Mathf.Atan2(stick.y, stick.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, stickAngle);
            return;
        }
        if (!Mouse.current.delta.ReadValue().Equals(Vector2.zero))
        {
            usingController = false;
        }
        if (usingController)
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
