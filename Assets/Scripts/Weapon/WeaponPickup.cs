using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public RaycastWeapon weaponPrefab;

    private void OnTriggerEnter(Collider other)
    {
        ActiveWeapon activeWeapon = other.gameObject.GetComponent<ActiveWeapon>();
        if (activeWeapon)
        {
            RaycastWeapon newWeapon = Instantiate(weaponPrefab);
            activeWeapon.Equip(newWeapon);
            Destroy(gameObject);
        }

        AIWeapon aiWeapon = other.gameObject.GetComponent<AIWeapon>();
        if (aiWeapon)
        {
            RaycastWeapon newWeapon = Instantiate(weaponPrefab);
            aiWeapon.Equip(newWeapon);
            Destroy(gameObject);
        }
    }
}