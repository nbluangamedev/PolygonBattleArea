using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ActiveWeapon : MonoBehaviour
{
    public enum WeaponSlot
    {
        Primary = 0,
        Secondary = 1
    }

    public Transform crossHairTarget;
    public Animator rigController;
    public Transform[] weaponSlots;
    public CharacterAiming characterAiming;
    public WeaponReload weaponReload;
    public bool isChangingWeapon;

    RaycastWeapon[] equippedWeapon = new RaycastWeapon[2];
    int activeWeaponIndex;
    public bool isHolstered = false;

    private void Start()
    {
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
        bool canFire = !isHolstered && notSprinting && !weaponReload.isReloading;

        if (weapon)
        {
            if (Input.GetButtonDown("Fire1") && canFire && !weapon.isFiring)
            {
                weapon.StartFiring();
            }

            weapon.UpdateWeapon(Time.deltaTime, crossHairTarget.position);

            if (Input.GetButtonUp("Fire1") || !canFire)
            {
                weapon.StopFiring();
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                ToggleActiveWeapon();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SetActiveWeapon(WeaponSlot.Primary);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SetActiveWeapon(WeaponSlot.Secondary);
            }
        }        
    }

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


    public void Equip(RaycastWeapon newWeapon)
    {
        int weaponSlotIndex = (int)newWeapon.weaponSlot;
        var weapon = GetWeapon(weaponSlotIndex);
        if (weapon)
        {
            Destroy(weapon.gameObject);
        }

        weapon = newWeapon;
        weapon.raycastDestination = crossHairTarget;
        weapon.recoil.characterAiming = characterAiming;
        weapon.recoil.rigController = rigController;
        weapon.transform.SetParent(weaponSlots[weaponSlotIndex], false);
        equippedWeapon[weaponSlotIndex] = weapon;

        SetActiveWeapon(newWeapon.weaponSlot);

        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.BroadCast(ListenType.UPDATE_AMMO, weapon.ammoCount);
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
                ListenerManager.Instance.BroadCast(ListenType.UPDATE_AMMO, weapon.ammoCount);
            }
        }
        isChangingWeapon = false;
    }
}