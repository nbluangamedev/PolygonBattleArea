using TMPro;
using UnityEngine;

public class ScreenGame : BaseScreen
{
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI ammoTotalText;
    [SerializeField] private TextMeshProUGUI healthText;
    public GameObject scopeOverlay;
    public GameObject crossHair;

    public override void Init()
    {
        base.Init();
        if (DataManager.HasInstance)
        {
            healthText.text = DataManager.Instance.globalConfig.playerMaxHealth.ToString();
        }
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.Register(ListenType.UPDATE_AMMO, OnUpdateAmmo);
            ListenerManager.Instance.Register(ListenType.SCOPE, OnUpdateScope);
            ListenerManager.Instance.Register(ListenType.ACTIVECROSSHAIR, OnUpdateDeactiveCrossHair);
            ListenerManager.Instance.Register(ListenType.UPDATE_HEALTH, OnUpdateHealth);
        }
    }

    private void OnDestroy()
    {
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.Unregister(ListenType.UPDATE_AMMO, OnUpdateAmmo);
            ListenerManager.Instance.Unregister(ListenType.SCOPE, OnUpdateScope);
            ListenerManager.Instance.Unregister(ListenType.ACTIVECROSSHAIR, OnUpdateDeactiveCrossHair);
            ListenerManager.Instance.Unregister(ListenType.UPDATE_HEALTH, OnUpdateHealth);
        }
    }

    private void OnUpdateHealth(object value)
    {
        if (value is PlayerHealth currentHealth)
        {
            float health = Mathf.Max(currentHealth.CurrentHealth, 0.0f);
            healthText.text = "Health: " + health.ToString();
        }
    }

    private void OnUpdateAmmo(object value)
    {
        if (value is RaycastWeapon weapon)
        {
            if (weapon.equipWeaponBy == EquipWeaponBy.Player)
            {
                ammoText.text = "Ammo: " + weapon.ammoCount.ToString();
                ammoTotalText.text = "Total Ammo: " + weapon.ammoTotal.ToString();
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