using System.Collections;
using UnityEngine;

public class AIWeapon : MonoBehaviour
{
    public RaycastWeapon AICurrentWeapon
    {
        get { return aiWeapons[currentWeaponIndex]; }
    }

    public WeaponSlot currentWeaponSlot
    {
        get { return (WeaponSlot)currentWeaponIndex; }
    }

    public float inAccuracy;
    public float inAccuracySniper;

    [SerializeField] private Transform currentTarget;
    [SerializeField] public RaycastWeapon[] aiWeapons = new RaycastWeapon[2];

    private int currentWeaponIndex = 0;
    private WeaponState weaponState = WeaponState.Holstered;
    private Animator animator;
    private WeaponIK weaponIK;
    private MeshSocketController socketController;
    private GameObject magazineHand;

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

    private void Awake()
    {
        animator = GetComponent<Animator>();
        weaponIK = GetComponent<WeaponIK>();
        socketController = GetComponent<MeshSocketController>();
    }

    private void Start()
    {
        foreach (var weapon in aiWeapons)
        {
            if (weapon)
            {
                currentWeaponIndex = (int)weapon.weaponSlot;
                break;
            }
        }
    }

    private void Update()
    {
        if (currentTarget && AICurrentWeapon && IsActive())
        {
            Vector3 target = currentTarget.position + weaponIK.targetOffset;

            if (AICurrentWeapon.weaponName.Equals("Sniper"))
            {
                target += Random.insideUnitSphere * inAccuracySniper;
                //WeaponRecoil(AICurrentWeapon.weaponName);
            }
            else
            {
                target += Random.insideUnitSphere * inAccuracy;
                //WeaponRecoil(AICurrentWeapon.weaponName);
            }
            AICurrentWeapon.UpdateWeapon(Time.deltaTime, target);
        }
    }

    public void SetTarget(Transform target)
    {
        weaponIK.SetTargetTransform(target);
        currentTarget = target;
    }

    public void SetFiring(bool enable)
    {
        if (enable)
        {
            AICurrentWeapon.StartFiring();
        }
        else
        {
            AICurrentWeapon.StopFiring();
        }
    }

    //public void WeaponRecoil(string weaponName)
    //{
    //    animator.Play("weapon_Recoil_" + weaponName);
    //}

    public bool HasWeapon()
    {
        int count = 0;
        foreach (RaycastWeapon weapon in aiWeapons)
        {
            if (weapon)
            {
                count++;
            }
        }

        if (count != 0)
        {
            return true;
        }
        else return false;
    }

    public int CountWeapon()
    {
        int count = 0;
        foreach (var weapon in aiWeapons)
        {
            if (weapon)
            {
                count++;
            }
        }
        return count;
    }

    public void Equip(RaycastWeapon weapon)
    {
        int weaponPickupSlot = (int)weapon.weaponSlot;
        if (aiWeapons[weaponPickupSlot])
        {
            DropWeapon(weaponPickupSlot);
        }

        currentWeaponIndex = weaponPickupSlot;
        aiWeapons[weaponPickupSlot] = weapon;

        if (weapon.weaponSlot == WeaponSlot.Primary)
        {
            socketController.Attach(weapon.transform, SocketID.Spine);
        }
        else
        {
            socketController.Attach(weapon.transform, SocketID.Hip);
        }
    }

    public void DropWeapon(int weaponDropSlot)
    {
        if (aiWeapons[weaponDropSlot])
        {
            RaycastWeapon currentWeapon = aiWeapons[weaponDropSlot];
            if (currentWeapon)
            {
                currentWeapon.transform.SetParent(null);
                currentWeapon.gameObject.GetComponent<BoxCollider>().enabled = true;
                foreach (Transform child in currentWeapon.transform)
                {
                    child.gameObject.layer = LayerMask.NameToLayer("Default");
                }
                currentWeapon.gameObject.AddComponent<Rigidbody>();
                aiWeapons[weaponDropSlot] = null;
                Destroy(currentWeapon.gameObject, 5f);
            }
        }
    }

    public void DropWeaponPrefab(int weaponDropSlot)
    {
        if (aiWeapons[weaponDropSlot])
        {
            RaycastWeapon currentWeapon = aiWeapons[weaponDropSlot];
            //Vector3 position = positionDropWeapon.TransformPoint(Vector3.forward);

            if (currentWeapon)
            {
                currentWeapon.transform.SetParent(null);
                aiWeapons[weaponDropSlot] = null;
                Destroy(currentWeapon.gameObject);

                string weaponName = currentWeapon.weaponName;
                switch (weaponName)
                {
                    case "Pistol":
                        //Instantiate(currentWeapon.weaponPrefabs[0], position, Quaternion.identity);
                        break;
                    case "Rifle":
                        //Instantiate(currentWeapon.weaponPrefabs[1], position, Quaternion.identity);
                        break;
                    case "Shotgun":
                        //Instantiate(currentWeapon.weaponPrefabs[2], position, Quaternion.identity);
                        break;
                    case "Sniper":
                        //Instantiate(currentWeapon.weaponPrefabs[3], position, Quaternion.identity);
                        break;
                }
            }
        }
    }

    public void ActivateWeapon()
    {
        RaycastWeapon weapon = AICurrentWeapon;
        if (weapon) StartCoroutine(EquipWeapon());

        //foreach (var weapon in aiWeapons)
        //{
        //    if (weapon)
        //    {
        //        currentWeaponIndex = (int)weapon.weaponSlot;
        //        StartCoroutine(EquipWeapon());
        //        break;
        //    }
        //}
    }

    private IEnumerator EquipWeapon()
    {
        weaponState = WeaponState.Activating;
        animator.runtimeAnimatorController = AICurrentWeapon.overrideAnimator;
        weaponIK.enabled = true;
        weaponIK.SetAimTransform(AICurrentWeapon.raycastOrigin);
        yield return new WaitForSeconds(.1f);
        animator.SetBool("equip", true);
        while (animator.GetCurrentAnimatorStateInfo(1).normalizedTime < 1.0f)
        {
            yield return null;
        }
        weaponState = WeaponState.Active;
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
        if ((int)AICurrentWeapon.weaponSlot == 0)
        {
            socketController.Attach(AICurrentWeapon.transform, SocketID.RightHandRifle);
        }
        else
        {
            socketController.Attach(AICurrentWeapon.transform, SocketID.RightHandPistol);
        }
    }

    private void HolsterWeaponEvent()
    {
        if ((int)AICurrentWeapon.weaponSlot == 0)
        {
            socketController.Attach(AICurrentWeapon.transform, SocketID.Spine);
        }
        else
        {
            socketController.Attach(AICurrentWeapon.transform, SocketID.Hip);
        }
    }

    private void DetachMagazine()
    {
        var leftHand = animator.GetBoneTransform(HumanBodyBones.LeftHand);
        RaycastWeapon weapon = AICurrentWeapon;
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
        RaycastWeapon weapon = AICurrentWeapon;
        weapon.magazine.SetActive(true);
        Destroy(magazineHand);
        weapon.RefillAmmo();
        animator.ResetTrigger("reload_Weapon");
    }

    public void RefillAmmo(int ammoAmount)
    {
        RaycastWeapon weapon = AICurrentWeapon;
        if (weapon)
        {
            weapon.ammoTotal += ammoAmount;
        }
    }

    public bool IsLowAmmo()
    {
        var weapon = AICurrentWeapon;
        if (weapon)
        {
            return weapon.IsEmptyAmmo();
        }
        return false;
    }
}