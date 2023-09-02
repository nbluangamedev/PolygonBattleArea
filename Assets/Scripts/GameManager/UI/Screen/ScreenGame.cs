using TMPro;
using UnityEngine;

public class ScreenGame : BaseScreen
{
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI ammoTotalText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI timeText;
    public GameObject scopeOverlay;
    public GameObject crossHair;

    public override void Init()
    {
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
        if (GameManager.HasInstance)
        {
            GameManager.Instance.timer = 0;
        }
        base.Init();
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

    public override void Show(object data)
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.timer = 0;
        }
        ammoText.text = "0";
        ammoTotalText.text = "0";
        crossHair.SetActive(true);
        base.Show(data);
    }

    public override void Hide()
    {
        if(GameManager.HasInstance)
        {
            GameManager.Instance.ResumeGame();
        }
        base.Hide();
    }

    private void OnUpdateHealth(object value)
    {
        if (value is PlayerHealth currentHealth)
        {
            float health = Mathf.Max(currentHealth.CurrentHealth, 0.0f);
            healthText.text = health.ToString();
            if (health <= 0.0f)
            {
                this.Hide();
            }
        }
    }

    private void OnUpdateAmmo(object value)
    {
        if (value is RaycastWeapon weapon)
        {
            if (weapon.equipWeaponBy == EquipWeaponBy.Player)
            {
                ammoText.text = weapon.ammoCount.ToString();
                ammoTotalText.text = weapon.ammoTotal.ToString();
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

    public void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}