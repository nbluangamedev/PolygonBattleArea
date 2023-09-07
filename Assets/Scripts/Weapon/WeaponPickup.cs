using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public RaycastWeapon weaponPrefab;

    private void OnTriggerEnter(Collider other)
    {
        ActiveWeapon activeWeapon = other.gameObject.GetComponent<ActiveWeapon>();
        if (activeWeapon)
        {
            if (activeWeapon.characterAiming.isAiming)
            {
                activeWeapon.characterAiming.UnScopeAndAim(activeWeapon.GetActiveWeapon());
            }

            int weaponPickupSlot = (int)weaponPrefab.weaponSlot;
            RaycastWeapon raycastWeapon = activeWeapon.GetWeapon(weaponPickupSlot);
            RaycastWeapon newWeapon = Instantiate(weaponPrefab);
            if (raycastWeapon)
            {
                string weaponPickupName = weaponPrefab.weaponName;
                int ammoTotal = raycastWeapon.ammoTotal;
                string weaponName = raycastWeapon.weaponName;
                if (weaponName == weaponPickupName)
                {
                    GetAmmoInWeapon(activeWeapon, ammoTotal, newWeapon);
                }
                else
                {
                    GetAmmoInWeapon(activeWeapon, 0, newWeapon);
                }
            }
            else
            {
                GetAmmoInWeapon(activeWeapon, 0, newWeapon);
            }
        }

        AIWeapon aiWeapon = other.gameObject.GetComponent<AIWeapon>();
        if (aiWeapon)
        {
            RaycastWeapon raycastWeapon = aiWeapon.AICurrentWeapon;
            RaycastWeapon newWeapon = Instantiate(weaponPrefab);
            if (raycastWeapon)
            {
                string weaponPickupName = weaponPrefab.weaponName;
                int ammoTotal = raycastWeapon.ammoTotal;
                string weaponName = raycastWeapon.weaponName;
                if (weaponName == weaponPickupName)
                {
                    GetAmmoInAIWeapon(aiWeapon, ammoTotal, newWeapon);
                }
                else
                {
                    GetAmmoInAIWeapon(aiWeapon, 0, newWeapon);
                }
            }
            else
            {
                GetAmmoInAIWeapon(aiWeapon, 0, newWeapon);
            }
        }
    }

    private void GetAmmoInAIWeapon(AIWeapon aiWeapon, int ammoTotal, RaycastWeapon newWeapon)
    {
        newWeapon.equipWeaponBy = EquipWeaponBy.AI;
        newWeapon.ammoTotal += ammoTotal;
        aiWeapon.Equip(newWeapon);
        Destroy(gameObject);
    }

    private void GetAmmoInWeapon(ActiveWeapon activeWeapon, int ammoTotal, RaycastWeapon newWeapon)
    {
        newWeapon.equipWeaponBy = EquipWeaponBy.Player;
        newWeapon.ammoTotal += ammoTotal;
        activeWeapon.Equip(newWeapon);
        Destroy(gameObject);
    }
}