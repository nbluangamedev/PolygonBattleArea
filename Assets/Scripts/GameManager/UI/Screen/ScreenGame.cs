using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScreenGame : BaseScreen
{
    [SerializeField] private TextMeshProUGUI ammoText;

    public override void Init()
    {
        base.Init();

        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.Register(ListenType.UPDATE_AMMO, OnUpdateAmmo);
        }
    }

    private void OnDestroy()
    {
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.Unregister(ListenType.UPDATE_AMMO, OnUpdateAmmo);
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
}
