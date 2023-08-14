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
        bool canReload = !characterLocomotion.isCrouching;

        if (weapon)
        {
            if (canReload && !weapon.IsEmptyAmmo())
            {
                if (Input.GetKeyDown(KeyCode.R) || weapon.ShouldReload())
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
}