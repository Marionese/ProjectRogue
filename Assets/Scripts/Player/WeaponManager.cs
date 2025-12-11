using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    public Transform weaponHolder;
    public GameObject startingWeaponPrefab;
    public WeaponBase currentWeapon; // reference to weapon on holder

    private bool attackPressed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EquipWeapon(startingWeaponPrefab);

    }
    //input


    void OnAim(InputValue val)
    {
        if (currentWeapon != null)
            currentWeapon.SetAim(val.Get<Vector2>());
    }

    void OnAttack(InputValue val)
    {
        attackPressed = val.isPressed;
    }
    // Update is called once per frame
    void Update()
    {
        if (attackPressed)
        {
            currentWeapon.Attack();
        }
    }
    public void EquipWeapon(GameObject weaponPrefab)
    {
        if (currentWeapon != null)
            Destroy(currentWeapon.gameObject);

        GameObject w = Instantiate(weaponPrefab, weaponHolder.position, Quaternion.identity, weaponHolder);
        currentWeapon = w.GetComponent<WeaponBase>();
        //set player owner to weapon
        var player = GetComponent<PlayerController>();
        currentWeapon.SetOwner(player);
    }
}
