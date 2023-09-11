using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerHealth : Health
{
    public Volume postProcessing;
    [Range(0, 1)] public float maxValue;
    public Camera mainCamera;

    private Ragdoll ragdoll;
    private ActiveWeapon activeWeapon;
    private CharacterAiming aiming;
    private CharacterLocomotion locomotion;

    protected override void OnStart()
    {
        if (DataManager.HasInstance)
        {
            maxHealth = DataManager.Instance.globalConfig.playerMaxHealth;
            maxAmor = DataManager.Instance.globalConfig.playerMaxAmor;
        }

        currentHealth = maxHealth;
        currentAmor = maxAmor;
        ragdoll = GetComponent<Ragdoll>();
        activeWeapon = GetComponent<ActiveWeapon>();
        aiming = GetComponent<CharacterAiming>();
        locomotion = GetComponent<CharacterLocomotion>();
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.BroadCast(ListenType.UPDATE_HEALTH, this);
        }
    }

    protected override void OnDamage(Vector3 direction, Rigidbody rigidBody)
    {
        UpdateVignette();
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.BroadCast(ListenType.UPDATE_HEALTH, this);
        }
    }

    protected override void OnHeal(float amount)
    {
        UpdateVignette();
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.BroadCast(ListenType.UPDATE_HEALTH, this);
        }
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_HEALTHPICKUP);
        }
    }

    private void UpdateVignette()
    {
        if (postProcessing.profile.TryGet(out Vignette vignette))
        {
            float percent = 1.0f - (currentHealth / maxHealth);
            vignette.intensity.value = percent * maxValue;
        }
    }

    protected override void OnDeath(Vector3 direction, Rigidbody ridigBody)
    {
        aiming.enabled = false;
        if (GameManager.HasInstance)
        {
            GameManager.Instance.ReleaseCursor();
        }
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.BroadCast(ListenType.SCOPE, false);
            ListenerManager.Instance.BroadCast(ListenType.ACTIVECROSSHAIR, false);
            ListenerManager.Instance.BroadCast(ListenType.ON_PLAYER_DEATH, this);
            ListenerManager.Instance.BroadCast(ListenType.UPDATE_HEALTH, this);
        }
        if (CameraManager.HasInstance)
        {
            CameraManager.Instance.EnableKillCam();
        }
        RaycastWeapon weapon = activeWeapon.GetActiveWeapon();
        if (weapon)
        {
            activeWeapon.DropWeaponPrefab((int)weapon.weaponSlot);
        }
        ragdoll.ActiveRagdoll();
        ragdoll.ApplyForce(direction, ridigBody);
        locomotion.enabled = false;
        if (postProcessing.profile.TryGet(out Vignette vignette))
        {
            vignette.intensity.value = 0f;
        }

        DOVirtual.DelayedCall(2f, ShowPopupWhenDie);
    }

    private void ShowPopupWhenDie()
    {
        if (UIManager.HasInstance)
        {
            UIManager.Instance.ShowPopup<PopupLose>();
            this.gameObject.SetActive(false);
        }
    }
}