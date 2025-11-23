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

        GameObject bullet = Instantiate(data.bulletPrefab, attackPoint.position, Quaternion.identity);
        Vector2 direction = (GetMouseWorldPos() - transform.position).normalized;

        // Bullet initialisieren
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
