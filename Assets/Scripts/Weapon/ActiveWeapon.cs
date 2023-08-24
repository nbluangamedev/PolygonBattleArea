using System.Collections;
using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    public WeaponSlot weaponSlot;

    public bool canFire;
    public bool isChangingWeapon;
    public bool isHolstered = false;
    public Animator rigController;
    public CharacterAiming characterAiming;
    public WeaponReload weaponReload;
    public Transform crossHairTarget;
    public Transform[] weaponSlots;

    private RaycastWeapon[] equippedWeapon = new RaycastWeapon[2];
    private int activeWeaponIndex = -1;

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
        RaycastWeapon weapon = GetWeapon(activeWeaponIndex);
        bool notSprinting = rigController.GetCurrentAnimatorStateInfo(2).shortNameHash == Animator.StringToHash("notSprinting");
        canFire = !isHolstered && notSprinting && !weaponReload.isReloading && !isChangingWeapon ;

        if (weapon)
        {
            if (Input.GetButtonDown("Fire1") && canFire && !weapon.isFiring)
            {
                weapon.StartFiring();
            }

            if (Input.GetButtonUp("Fire1") || !canFire || weapon.IsEmptyAmmo() || weapon.ammoCount <= 0)
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

            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (activeWeaponIndex == 0)
                {
                    if (GetWeapon(1))
                    {
                        SetActiveWeapon(WeaponSlot.Secondary);
                    }
                }

                if (activeWeaponIndex == 1)
                {
                    if (GetWeapon(0))
                    {
                        SetActiveWeapon(WeaponSlot.Primary);
                    }
                }
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

    public void Equip(RaycastWeapon newWeapon)
    {
        int weaponSlotIndex = (int)newWeapon.weaponSlot;

        RaycastWeapon equipWeapon = GetWeapon(weaponSlotIndex);

        if (equipWeapon)
        {
            DropWeapon(weaponSlotIndex);
        }

        equipWeapon = newWeapon;
        equipWeapon.recoil.characterAiming = characterAiming;
        equipWeapon.recoil.rigController = rigController;
        equipWeapon.transform.SetParent(weaponSlots[weaponSlotIndex], false);
        equippedWeapon[weaponSlotIndex] = equipWeapon;
        SetActiveWeapon(newWeapon.weaponSlot);

        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.BroadCast(ListenType.UPDATE_AMMO, equipWeapon);
        }
    }

    public void DropWeapon(int weaponDropSlot)
    {
        RaycastWeapon currentWeapon = GetWeapon(weaponDropSlot);
        if (currentWeapon)
        {
            currentWeapon.transform.SetParent(null);
            currentWeapon.gameObject.GetComponent<BoxCollider>().enabled = true;
            foreach (Transform child in currentWeapon.transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("Default");
            }
            currentWeapon.gameObject.AddComponent<Rigidbody>();
            equippedWeapon[weaponDropSlot] = null;
            Destroy(currentWeapon, 5f);
        }
    }

    public RaycastWeapon GetActiveWeapon()
    {
        return GetWeapon(activeWeaponIndex);
    }

    private RaycastWeapon GetWeapon(int index)
    {
        if (index < 0 || index >= equippedWeapon.Length)
        {
            return null;
        }
        return equippedWeapon[index];
    }

    private void ToggleActiveWeapon()
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

    private void SetActiveWeapon(WeaponSlot weaponSlot)
    {
        int holsterIndex = activeWeaponIndex;
        int activateIndex = (int)weaponSlot;

        if (holsterIndex == activateIndex)
        {
            holsterIndex = -1;
        }

        StartCoroutine(SwitchWeapon(holsterIndex, activateIndex));
    }

    private IEnumerator SwitchWeapon(int holsterIndex, int activateIndex)
    {
        rigController.SetInteger("weapon_Index", activateIndex);
        yield return StartCoroutine(HolsterWeapon(holsterIndex));
        yield return StartCoroutine(ActivateWeapon(activateIndex));
        activeWeaponIndex = activateIndex;
    }

    private IEnumerator HolsterWeapon(int index)
    {
        isChangingWeapon = true;
        isHolstered = true;
        var weapon = GetWeapon(index);
        if (weapon)
        {
            rigController.SetBool("holster_Weapon", true);

            if (ListenerManager.HasInstance)
            {
                ListenerManager.Instance.BroadCast(ListenType.ACTIVECROSSHAIR, true);
            }

            yield return new WaitForSeconds(0.1f);
            do
            {
                yield return new WaitForEndOfFrame();
            } while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        }
        isChangingWeapon = false;
    }

    private IEnumerator ActivateWeapon(int index)
    {
        isChangingWeapon = true;
        var weapon = GetWeapon(index);
        if (weapon)
        {
            rigController.SetBool("holster_Weapon", false);
            rigController.Play("equip_" + weapon.weaponName);

            if (ListenerManager.HasInstance)
            {
                if (weapon.weaponName == "Sniper")
                {
                    ListenerManager.Instance.BroadCast(ListenType.ACTIVECROSSHAIR, false);
                }
                else
                {
                    ListenerManager.Instance.BroadCast(ListenType.ACTIVECROSSHAIR, true);
                }
            }

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

    public void RefillAmmo(int ammoAmount)
    {
        RaycastWeapon weapon = GetActiveWeapon();
        if (weapon)
        {
            weapon.ammoTotal += ammoAmount;
            if (ListenerManager.HasInstance)
            {
                ListenerManager.Instance.BroadCast(ListenType.UPDATE_AMMO, weapon);
            }
        }
    }
}