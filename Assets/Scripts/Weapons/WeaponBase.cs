using UnityEngine;
using UnityEngine.InputSystem;
public abstract class WeaponBase : MonoBehaviour
{
    public WeaponData data;           // ScriptableObject Daten
    public Transform attackPoint;     // BulletSpawn / Slash-Point
    protected bool canShoot = true;
    private float timer;
    protected Vector2 aimInput;
    protected bool usingController;
    protected bool allowMouse;
    protected Camera cam;
    // Jedes Weapon-Kind muss Attack() haben:
    public abstract void Attack();
    public void SetAim(Vector2 input)
    {
        aimInput = input;

        if (input.sqrMagnitude > 0.1f)
            usingController = true;
    }
    void Start()
    {

        cam = Camera.main;
        var input = GetComponentInParent<PlayerInput>();
        allowMouse = input != null && input.currentControlScheme == "Keyboard&Mouse";
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
    protected Vector3 GetMouseWorldPos()
    {
        return cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }
}

