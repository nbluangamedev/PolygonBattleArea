using UnityEngine;
using UnityEngine.UI;

public class WeaponPickupUI : MonoBehaviour
{
    public Sprite[] sprites;
    public Transform primary, secondary;
    private Color alphaColor = new Color32(255, 255, 255, 60);
    private Color normalColor = new Color32(255, 255, 255, 255);

    private void Start()
    {
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.Register(ListenType.HOLSTER_WEAPON_UI, UpdateHolsterWeaponUI);
            ListenerManager.Instance.Register(ListenType.DROP_WEAPON_UI, UpdateDropWeaponUI);
            ListenerManager.Instance.Register(ListenType.ACTIVE_WEAPON_UI, UpdateActiveWeaponUI);
        }
        primary = transform.Find("Primary");
        secondary = transform.Find("Secondary");

        primary.GetComponent<Image>().sprite = sprites[0];
        primary.GetComponent<Image>().color = alphaColor;
        secondary.GetComponent<Image>().sprite = sprites[1];
        secondary.GetComponent<Image>().color = alphaColor;
    }

    private void OnDestroy()
    {
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.Unregister(ListenType.HOLSTER_WEAPON_UI, UpdateHolsterWeaponUI);
            ListenerManager.Instance.Unregister(ListenType.DROP_WEAPON_UI, UpdateDropWeaponUI);
            ListenerManager.Instance.Unregister(ListenType.ACTIVE_WEAPON_UI, UpdateActiveWeaponUI);
        }
    }

    private void UpdateHolsterWeaponUI(object weapon)
    {
        if (weapon is RaycastWeapon raycastWeapon)
        {
            int slot = (int)raycastWeapon.weaponSlot;
            switch (slot)
            {
                case 0:
                    primary.GetComponent<Image>().sprite = sprites[0];
                    primary.GetComponent<Image>().color = normalColor;
                    break;
                case 1:
                    secondary.GetComponent<Image>().sprite = sprites[1];
                    secondary.GetComponent<Image>().color = normalColor;
                    break;
            }
        }
    }

    private void UpdateDropWeaponUI(object weapon)
    {
        if (weapon is RaycastWeapon raycastWeapon)
        {
            int slot = (int)raycastWeapon.weaponSlot;
            switch (slot)
            {
                case 0:
                    primary.GetComponent<Image>().sprite = sprites[0];
                    primary.GetComponent<Image>().color = alphaColor;
                    break;
                case 1:
                    secondary.GetComponent<Image>().sprite = sprites[1];
                    secondary.GetComponent<Image>().color = alphaColor;
                    break;
            }
        }
    }

    private void UpdateActiveWeaponUI(object weapon)
    {
        if (weapon is RaycastWeapon raycastWeapon)
        {
            int slot = (int)raycastWeapon.weaponSlot;
            switch (slot)
            {
                case 0:
                    primary.GetComponent<Image>().sprite = sprites[2];
                    primary.GetComponent<Image>().color = normalColor;
                    break;
                case 1:
                    secondary.GetComponent<Image>().sprite = sprites[3];
                    secondary.GetComponent<Image>().color = normalColor;
                    break;
            }
        }
    }
}
