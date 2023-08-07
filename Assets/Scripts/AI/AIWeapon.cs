using System.Collections;
using UnityEngine;

public class AIWeapon : MonoBehaviour
{
    public RaycastWeapon currentWeapon
    {
        get { return weapons[currentWeaponIndex]; }
    }
    private RaycastWeapon[] weapons = new RaycastWeapon[2];
    private int currentWeaponIndex = 0;

    private WeaponState weaponState = WeaponState.Holstered;
    private Animator animator;
    private MeshSocketController socketController;
    private WeaponIK weaponIK;
    private Transform currentTarget;
    private float inAccuracy;
    private float inAccuracySniper;
    private GameObject magazineHand;

    public float dropForce = 1.5f;

    public bool IsActive()
    {
        return weaponState == WeaponState.Active;
    }

    public bool IsHolstered()
    {
        return weaponState == WeaponState.Holstered;
    }

    public bool IsReloading()
    {
        return weaponState == WeaponState.Reloading;
    }

    public WeaponSlot currentWeaponSlot
    {
        get { return (WeaponSlot)currentWeaponIndex; }
    }

    private void Awake()
    {
        if (DataManager.HasInstance)
        {
            inAccuracy = DataManager.Instance.globalConfig.inAccuracy;
            inAccuracySniper = DataManager.Instance.globalConfig.inAccuracySniper;
        }

        animator = GetComponent<Animator>();
        weaponIK = GetComponent<WeaponIK>();
        socketController = GetComponent<MeshSocketController>();
    }

    private void Update()
    {
        if (currentTarget && currentWeapon && IsActive())
        {
            Vector3 target = currentTarget.position + weaponIK.targetOffset;
            if (currentWeapon.weaponName.Equals("Sniper"))
            {
                target += Random.insideUnitSphere * inAccuracySniper;
            }
            target += Random.insideUnitSphere * inAccuracy;
            currentWeapon.UpdateWeapon(Time.deltaTime, target);
        }
    }

    public void SetFiring(bool enable)
    {
        if (enable)
        {
            currentWeapon.StartFiring();
            if (currentWeapon.weaponName.Equals("Sniper"))
            {
                int ammo = currentWeapon.ammoCount;
                if (ammo > currentWeapon.ammoCount)
                {
                    animator.Play("sniperPullBolt");
                    ammo -= 1;
                }
            }
        }
        else
        {
            currentWeapon.StopFiring();
        }
    }

    public void Equip(RaycastWeapon weapon)
    {
        weapons[(int)weapon.weaponSlot] = weapon;

        if (weapon.weaponSlot == WeaponSlot.Primary)
        {
            socketController.Attach(weapon.transform, SocketID.Spine);
        }
        else
        {
            socketController.Attach(weapon.transform, SocketID.Hip);
        }
    }

    public void ActivateWeapon()
    {
        StartCoroutine(EquipWeapon());
    }

    private IEnumerator EquipWeapon()
    {
        weaponState = WeaponState.Activating;
        animator.runtimeAnimatorController = currentWeapon.overrideAnimator;
        animator.SetBool("equip", true);
        yield return new WaitForSeconds(.5f);
        while (animator.GetCurrentAnimatorStateInfo(1).normalizedTime < 1.0f)
        {
            yield return null;
        }
        weaponIK.enabled = true;
        weaponIK.SetAimTransform(currentWeapon.raycastOrigin);
        weaponState = WeaponState.Active;
    }

    public int CountWeapon()
    {
        int count = 0;
        foreach (var weapon in weapons)
        {
            if (weapon != null)
            {
                count++;
            }
        }
        return count;
    }

    public void SwitchWeapon(WeaponSlot slot)
    {
        if (IsHolstered())
        {
            currentWeaponIndex = (int)slot;
            ActivateWeapon();
            return;
        }

        int equipIndex = (int)slot;
        if (IsActive() && currentWeaponIndex != equipIndex)
        {
            StartCoroutine(SwitchWeaponAI(equipIndex));
        }
    }

    private IEnumerator SwitchWeaponAI(int index)
    {
        yield return StartCoroutine(Holster());
        currentWeaponIndex = index;
        yield return StartCoroutine(EquipWeapon());
    }

    public void DeActivateWeapon()
    {
        SetTarget(null);
        SetFiring(false);
        StartCoroutine(Holster());
    }

    private IEnumerator Holster()
    {
        weaponState = WeaponState.Holstering;
        animator.SetBool("equip", false);
        weaponIK.enabled = false;
        yield return new WaitForSeconds(0.5f);
        while (animator.GetCurrentAnimatorStateInfo(1).normalizedTime < 1.0f)
        {
            yield return null;
        }
        weaponState = WeaponState.Holstered;
    }

    public void ReloadWeapon()
    {
        if (IsActive())
        {
            StartCoroutine(Reaload());
        }
    }

    private IEnumerator Reaload()
    {
        weaponState = WeaponState.Reloading;
        animator.SetTrigger("reload_Weapon");
        weaponIK.enabled = false;
        yield return new WaitForSeconds(0.5f);
        while (animator.GetCurrentAnimatorStateInfo(1).normalizedTime < 1.0f)
        {
            yield return null;
        }
        weaponIK.enabled = true;
        weaponState = WeaponState.Active;
    }

    public bool HasWeapon()
    {
        return currentWeapon != null;
    }

    public void DropWeapon()
    {
        if (currentWeapon)
        {
            currentWeapon.transform.SetParent(null);
            currentWeapon.gameObject.GetComponent<BoxCollider>().enabled = true;
            currentWeapon.gameObject.AddComponent<Rigidbody>();
            weapons[currentWeaponIndex] = null;
        }
    }

    public void SetTarget(Transform target)
    {
        weaponIK.SetTargetTransform(target);
        currentTarget = target;
    }

    public void OnAnimationEvent(string eventName)
    {
        switch (eventName)
        {
            case "equipWeapon":
                EquipWeaponEvent();
                break;
            case "holsterWeapon":
                HolsterWeaponEvent();
                break;
            case "detach_Magazine":
                DetachMagazine();
                break;
            case "drop_Magazine":
                DropMagazine();
                break;
            case "refill_Magazine":
                RefillMagazine();
                break;
            case "attach_Magazine":
                AttachMagazine();
                break;
        }
    }

    private void EquipWeaponEvent()
    {
        if ((int)currentWeapon.weaponSlot == 0)
        {
            socketController.Attach(currentWeapon.transform, SocketID.RightHandRifle);
        }
        else
        {
            socketController.Attach(currentWeapon.transform, SocketID.RightHandPistol);
        }
    }
    private void HolsterWeaponEvent()
    {
        if ((int)currentWeapon.weaponSlot == 0)
        {
            socketController.Attach(currentWeapon.transform, SocketID.Spine);

        }
        else
        {
            socketController.Attach(currentWeapon.transform, SocketID.Hip);

        }
    }

    private void DetachMagazine()
    {
        var leftHand = animator.GetBoneTransform(HumanBodyBones.LeftHand);
        RaycastWeapon weapon = currentWeapon;
        magazineHand = Instantiate(weapon.magazine, leftHand, true);
        weapon.magazine.SetActive(false);
    }

    private void DropMagazine()
    {
        GameObject droppedMagazine = Instantiate(magazineHand, magazineHand.transform.position, magazineHand.transform.rotation);
        droppedMagazine.transform.localScale = Vector3.one;
        droppedMagazine.AddComponent<Rigidbody>();
        droppedMagazine.AddComponent<BoxCollider>();
        magazineHand.SetActive(false);
        Destroy(droppedMagazine, 5f);
        droppedMagazine.hideFlags = HideFlags.HideInHierarchy;
    }
    private void RefillMagazine()
    {
        magazineHand.SetActive(true);
    }
    private void AttachMagazine()
    {
        RaycastWeapon weapon = currentWeapon;
        weapon.magazine.SetActive(true);
        Destroy(magazineHand);
        weapon.ammoCount = weapon.clipSize;
        animator.ResetTrigger("reload_Weapon");
    }
}