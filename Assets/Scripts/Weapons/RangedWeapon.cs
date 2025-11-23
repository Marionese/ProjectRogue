using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;


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

        // Aktuelle Richtung basierend auf aktivem Modus
        Vector2 direction;

        if (usingController)
        {
            if (aimInput.sqrMagnitude > 0.1f)
                direction = aimInput;  // Stick aktiv
            else
                direction = transform.right;  // letzte Waffenrotation verwenden
        }
        else
        {
            direction = allowMouse ? (GetMouseWorldPos() - attackPoint.position) : Vector2.zero;
        }

        if (direction == Vector2.zero)
            return;

        // Wenn zu nah am Spieler, nichts schießen
        if (direction.sqrMagnitude < 0.22f)
            return;

        direction.Normalize();

        // Kleiner Offset, damit Kugeln nicht im Spieler spawnen
        Vector3 spawnPos = attackPoint.position + (Vector3)(direction * 0.1f);

        GameObject bullet = Instantiate(data.bulletPrefab, spawnPos, Quaternion.identity);
        bullet.GetComponent<BulletScript>().Initialize(data.damage, data.bulletSpeed, direction);
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
