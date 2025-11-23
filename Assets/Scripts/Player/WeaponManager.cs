using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    public Transform weaponHolder;
    public GameObject startingWeaponPrefab;
    public WeaponBase currentWeapon; // reference to weapon on holder
    InputAction shootAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EquipWeapon(startingWeaponPrefab);
        shootAction = InputSystem.actions.FindAction("Shoot");

    }

    // Update is called once per frame
    void Update()
    {
        if (shootAction.IsPressed())
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
    }
}
