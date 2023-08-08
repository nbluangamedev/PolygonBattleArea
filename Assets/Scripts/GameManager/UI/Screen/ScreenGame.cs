using Cinemachine;
using TMPro;
using UnityEngine;

public class ScreenGame : BaseScreen
{
    [SerializeField] private TextMeshProUGUI ammoText;
    public GameObject scopeOverlay;
    public GameObject crossHair;

    public override void Init()
    {
        base.Init();

        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.Register(ListenType.UPDATE_AMMO, OnUpdateAmmo);
            ListenerManager.Instance.Register(ListenType.SCOPE, OnUpdateScope);
            ListenerManager.Instance.Register(ListenType.ACTIVECROSSHAIR, OnUpdateDeactiveCrossHair);
        }
    }

    private void OnDestroy()
    {
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.Unregister(ListenType.UPDATE_AMMO, OnUpdateAmmo);
            ListenerManager.Instance.Unregister(ListenType.SCOPE, OnUpdateScope);
            ListenerManager.Instance.Unregister(ListenType.ACTIVECROSSHAIR, OnUpdateDeactiveCrossHair);
        }
    }

    private void OnUpdateAmmo(object value)
    {
        if (value is RaycastWeapon weapon)
        {
            if (weapon.equipWeaponBy == EquipWeaponBy.Player)
            {
                ammoText.text = weapon.ammoCount.ToString();
            }
        }
    }

    private void OnUpdateScope(object value)
    {
        if (value is bool active)
        {
            scopeOverlay.SetActive(active);
        }
    }

    private void OnUpdateDeactiveCrossHair(object value)
    {
        if (value is bool active)
        {
            crossHair.SetActive(active);
        }
    }
}