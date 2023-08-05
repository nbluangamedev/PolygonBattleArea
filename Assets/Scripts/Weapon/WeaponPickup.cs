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
            newWeapon.equipWeaponBy = EquipWeaponBy.Player;
            activeWeapon.Equip(newWeapon);
            Destroy(gameObject);
        }

        AIWeapon aiWeapon = other.gameObject.GetComponent<AIWeapon>();
        if (aiWeapon)
        {
            RaycastWeapon newWeapon = Instantiate(weaponPrefab);
            newWeapon.equipWeaponBy = EquipWeaponBy.AI;
            aiWeapon.Equip(newWeapon);
            SphereCollider sphereCollider = aiWeapon.gameObject.GetComponent<SphereCollider>();
            Destroy(sphereCollider);
            Destroy(gameObject);
        }
    }
}