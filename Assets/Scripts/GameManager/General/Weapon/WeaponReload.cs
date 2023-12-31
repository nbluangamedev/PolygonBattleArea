using UnityEngine;

public class WeaponReload : MonoBehaviour
{
    public WeaponAnimationEvent animationEvents;
    public Animator rigController;
    public ActiveWeapon activeWeapon;

    public Transform leftHand;
    public bool isReloading;

    private GameObject magazineHand;
    private CharacterLocomotion characterLocomotion;

    private void Start()
    {
        animationEvents.weaponAnimationEvent.AddListener(OnAnimationEvent);
        characterLocomotion = GetComponent<CharacterLocomotion>();
    }

    private void Update()
    {
        RaycastWeapon weapon = activeWeapon.GetActiveWeapon();

        if (weapon)
        {
            bool canReload = !characterLocomotion.isCrouching && weapon.ammoCount < weapon.clipSize;
            if (canReload)
            {
                if ((Input.GetKeyDown(KeyCode.R) && weapon.ammoTotal > 0) || weapon.ShouldReload())
                {
                    isReloading = true;
                    rigController.SetTrigger("reload_Weapon");
                }
            }
        }
    }

    private void OnAnimationEvent(string eventName)
    {
        switch (eventName)
        {
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
            case "PistolShoot":
                PistolShoot();
                break;
            case "PistolReload":
                PistolReload();
                break;
            case "RifleShoot":
                RifleShoot();
                break;
            case "RifleReload":
                RifleReload();
                break;
            case "ShotgunShoot":
                ShotgunShoot();
                break;
            case "ShotgunRefillMagazine":
                ShotgunRefillMagazine();
                break;
            case "ShotgunAttachMagazine":
                ShotgunAttachMagazine();
                break;
            case "SniperShoot":
                SniperShoot();
                break;
            case "PullBolt":
                PullBolt();
                break;
            case "SniperDetachMagazine":
                SniperDetachMagazine();
                break;
            case "SniperAttachMagazine":
                SniperAttachMagazine();
                break;

        }
    }

    private void DetachMagazine()
    {
        RaycastWeapon weapon = activeWeapon.GetActiveWeapon();
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
        RaycastWeapon weapon = activeWeapon.GetActiveWeapon();
        weapon.magazine.SetActive(true);
        Destroy(magazineHand);
        weapon.RefillAmmo();
        rigController.ResetTrigger("reload_Weapon");
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.BroadCast(ListenType.UPDATE_AMMO, weapon);
        }
        isReloading = false;
    }

    private void PistolShoot()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_PISTOL);
        }
    }

    private void PistolReload()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_PISTOLRELOAD);
        }
    }

    private void RifleShoot()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_RIFLE1);
        }
    }

    private void RifleReload()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_GENERIC_RELOAD);
        }
    }

    private void ShotgunShoot()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_SHOTGUN1);
        }
    }

    private void ShotgunRefillMagazine()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_SHOTGUNREFILL);
        }
    }

    private void ShotgunAttachMagazine()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_SHOTGUNATTACH);
        }
    }

    private void SniperShoot()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_SNIPER);
        }
    }

    private void PullBolt()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_SNIPERBOLT);
        }
    }

    private void SniperDetachMagazine()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_SNIPERDETACHMAGAZINE);
        }
    }

    private void SniperAttachMagazine()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_SNIPERATTACHMAGAZINE);
        }
    }
}