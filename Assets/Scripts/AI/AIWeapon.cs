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

    public Transform positionDropWeapon;

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
        positionDropWeapon = GetComponentInChildren<Transform>().Find("PositionDropWeapon");
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
            }
            else
            {
                target += Random.insideUnitSphere * inAccuracy;
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
            DropWeaponPrefab(weaponPickupSlot);
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
            Vector3 position = positionDropWeapon.TransformPoint(Vector3.forward);

            if (currentWeapon)
            {
                int ammoCount = currentWeapon.ammoCount;
                int ammoTotal = currentWeapon.ammoTotal;
                currentWeapon.transform.SetParent(null);
                aiWeapons[weaponDropSlot] = null;
                Destroy(currentWeapon.gameObject);

                string weaponName = currentWeapon.weaponName;
                switch (weaponName)
                {
                    case "Pistol":
                        GameObject dropWeapon = Instantiate(currentWeapon.weaponPickupPrefabs[0], position, Quaternion.identity);
                        dropWeapon.GetComponent<WeaponPickup>().weaponPrefab.ammoCount = ammoCount;
                        dropWeapon.GetComponent<WeaponPickup>().weaponPrefab.ammoTotal = ammoTotal;
                        break;
                    case "Rifle":
                        GameObject dropWeapon1 = Instantiate(currentWeapon.weaponPickupPrefabs[1], position, Quaternion.identity);
                        dropWeapon1.GetComponent<WeaponPickup>().weaponPrefab.ammoCount = ammoCount;
                        dropWeapon1.GetComponent<WeaponPickup>().weaponPrefab.ammoTotal = ammoTotal;
                        break;
                    case "Shotgun":
                        GameObject dropWeapon2 = Instantiate(currentWeapon.weaponPickupPrefabs[2], position, Quaternion.identity);
                        dropWeapon2.GetComponent<WeaponPickup>().weaponPrefab.ammoCount = ammoCount;
                        dropWeapon2.GetComponent<WeaponPickup>().weaponPrefab.ammoTotal = ammoTotal;
                        break;
                    case "Sniper":
                        GameObject dropWeapon3 = Instantiate(currentWeapon.weaponPickupPrefabs[3], position, Quaternion.identity);
                        dropWeapon3.GetComponent<WeaponPickup>().weaponPrefab.ammoCount = ammoCount;
                        dropWeapon3.GetComponent<WeaponPickup>().weaponPrefab.ammoTotal = ammoTotal;
                        break;
                }
            }
        }
    }

    public void ActivateWeapon()
    {
        RaycastWeapon weapon = AICurrentWeapon;
        if (weapon) StartCoroutine(EquipWeapon());
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
        AIEquipWeapon();
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
        RifleReload();
        weaponIK.enabled = true;
        weaponState = WeaponState.Active;
    }

    public void OnAnimationEventAI(string eventName)
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
            case "AIEquipWeapon":
                AIEquipWeapon();
                break;
            case "rifleReload":
                RifleReload();
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

    private void AIEquipWeapon()
    {
        if (AudioManager.HasInstance)
        {
            string weaponName = AICurrentWeapon.weaponName;
            switch (weaponName)
            {
                case "Pistol":
                    AudioManager.Instance.PlaySEAgent(AUDIO.SE_PISTOLEQUIP, AudioManager.Instance.AttachSESource.volume);
                    break;
                case "Rifle":
                    AudioManager.Instance.PlaySEAgent(AUDIO.SE_RIFLEEQUIP, AudioManager.Instance.AttachSESource.volume);
                    break;
                case "Shotgun":
                    AudioManager.Instance.PlaySEAgent(AUDIO.SE_SHOTGUNEQUIP, AudioManager.Instance.AttachSESource.volume);
                    break;
                case "Sniper":
                    AudioManager.Instance.PlaySEAgent(AUDIO.SE_SNIPERBOLT, AudioManager.Instance.AttachSESource.volume);
                    break;
            }
        }
    }

    private void RifleReload()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySEAgent(AUDIO.SE_GENERIC_RELOAD, AudioManager.Instance.AttachSESource.volume);
        }
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