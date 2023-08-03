using System.Collections;
using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    public WeaponSlot weaponSlot;

    public Transform crossHairTarget;
    public Animator rigController;
    public Transform[] weaponSlots;
    public CharacterAiming characterAiming;
    public WeaponReload weaponReload;
    public bool isChangingWeapon;
    public bool canFire;
    public bool isHolstered = false;

    RaycastWeapon[] equippedWeapon = new RaycastWeapon[2];
    int activeWeaponIndex;
    //Ray rayPickupWeapon;
    //RaycastHit hitBoxCollider;
    //Camera mainCamera;

    private void Start()
    {
        //mainCamera = Camera.main;
        weaponReload = GetComponent<WeaponReload>();
        RaycastWeapon existingWeapon = GetComponentInChildren<RaycastWeapon>();
        if (existingWeapon)
        {
            Equip(existingWeapon);
        }
    }

    private void Update()
    {
        var weapon = GetWeapon(activeWeaponIndex);
        bool notSprinting = rigController.GetCurrentAnimatorStateInfo(2).shortNameHash == Animator.StringToHash("notSprinting");
        canFire = !isHolstered && notSprinting && !weaponReload.isReloading;

        if (weapon)
        {
            if (Input.GetButtonDown("Fire1") && canFire && !weapon.isFiring)
            {
                weapon.StartFiring();
            }

            if (Input.GetButtonUp("Fire1") || !canFire)
            {
                weapon.StopFiring();
            }

            weapon.UpdateWeapon(Time.deltaTime, crossHairTarget.position);

            if (Input.GetKeyDown(KeyCode.X))
            {
                ToggleActiveWeapon();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (GetWeapon(0))
                {
                    SetActiveWeapon(WeaponSlot.Primary);
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (GetWeapon(1))
                {
                    SetActiveWeapon(WeaponSlot.Secondary);
                }
            }

            //if (Input.GetKeyDown(KeyCode.G))
            //{
            //    TriggerBoxCollider();
            //}
        }
    }

    //private void TriggerBoxCollider()
    //{
    //    rayPickupWeapon.origin = mainCamera.transform.position;
    //    rayPickupWeapon.direction = mainCamera.transform.forward;

    //    Debug.DrawLine(rayPickupWeapon.origin, rayPickupWeapon.direction, Color.red);

    //    if (Physics.Raycast(rayPickupWeapon, out hitBoxCollider, 100f, LayerMask.NameToLayer("Weapon")))
    //    {
    //        if(hitBoxCollider.collider.GetComponent<BoxCollider>());
    //            hitBoxCollider.collider.isTrigger = true;
    //    }
    //}

    public bool IsFiring()
    {
        RaycastWeapon currentWeapon = GetActiveWeapon();
        if (!currentWeapon)
        {
            return false;
        }
        else
        {
            return currentWeapon.isFiring;
        }
    }

    public RaycastWeapon GetActiveWeapon()
    {
        return GetWeapon(activeWeaponIndex);
    }

    RaycastWeapon GetWeapon(int index)
    {
        if (index < 0 || index >= equippedWeapon.Length)
        {
            return null;
        }
        return equippedWeapon[index];
    }

    public void DropWeapon()
    {
        var currentWeapon = GetActiveWeapon();
        if (currentWeapon)
        {
            currentWeapon.transform.SetParent(null);
            currentWeapon.gameObject.GetComponent<BoxCollider>().enabled = true;
            currentWeapon.gameObject.AddComponent<Rigidbody>();
            //currentWeapon.gameObject.AddComponent<WeaponPickup>().weaponPrefab = currentWeapon;
            foreach (Transform child in currentWeapon.transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("Default");
            }
            equippedWeapon[activeWeaponIndex] = null;
        }
    }

    public void Equip(RaycastWeapon newWeapon)
    {
        int weaponSlotIndex = (int)newWeapon.weaponSlot;
        var weapon = GetWeapon(weaponSlotIndex);
        if (weapon)
        {
            //Destroy(weapon.gameObject);
            DropWeapon();
        }

        weapon = newWeapon;
        //weapon.raycastDestination = crossHairTarget;
        weapon.recoil.characterAiming = characterAiming;
        weapon.recoil.rigController = rigController;
        weapon.transform.SetParent(weaponSlots[weaponSlotIndex], false);
        equippedWeapon[weaponSlotIndex] = weapon;

        SetActiveWeapon(newWeapon.weaponSlot);

        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.BroadCast(ListenType.UPDATE_AMMO, weapon);
        }
    }

    void ToggleActiveWeapon()
    {
        bool isHolstered = rigController.GetBool("holster_Weapon");
        if (isHolstered)
        {
            StartCoroutine(ActivateWeapon(activeWeaponIndex));
        }
        else
        {
            StartCoroutine(HolsterWeapon(activeWeaponIndex));
        }
    }

    void SetActiveWeapon(WeaponSlot weaponSlot)
    {
        int holsterIndex = activeWeaponIndex;
        int activateIndex = (int)weaponSlot;

        if (holsterIndex == activateIndex)
        {
            holsterIndex = -1;
        }

        StartCoroutine(SwitchWeapon(holsterIndex, activateIndex));
    }

    IEnumerator SwitchWeapon(int holsterIndex, int activateIndex)
    {
        rigController.SetInteger("weapon_Index", activateIndex);
        yield return StartCoroutine(HolsterWeapon(holsterIndex));
        yield return StartCoroutine(ActivateWeapon(activateIndex));
        activeWeaponIndex = activateIndex;
    }

    IEnumerator HolsterWeapon(int index)
    {
        isChangingWeapon = true;
        isHolstered = true;
        var weapon = GetWeapon(index);
        if (weapon)
        {
            rigController.SetBool("holster_Weapon", true);
            yield return new WaitForSeconds(0.1f);
            do
            {
                yield return new WaitForEndOfFrame();
            } while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        }
        isChangingWeapon = false;
    }

    IEnumerator ActivateWeapon(int index)
    {
        isChangingWeapon = true;
        var weapon = GetWeapon(index);
        if (weapon)
        {
            rigController.SetBool("holster_Weapon", false);
            rigController.Play("equip_" + weapon.weaponName);
            yield return new WaitForSeconds(0.1f);
            do
            {
                yield return new WaitForEndOfFrame();
            } while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
            isHolstered = false;
            if (ListenerManager.HasInstance)
            {
                ListenerManager.Instance.BroadCast(ListenType.UPDATE_AMMO, weapon);
            }
        }
        isChangingWeapon = false;
    }
}