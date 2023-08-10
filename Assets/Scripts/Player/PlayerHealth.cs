using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerHealth : Health
{
    public Volume postProcessing;
    [Range(0, 1)] public float maxValue;

    private Ragdoll ragdoll;
    private ActiveWeapon activeWeapon;
    private CharacterAiming aiming;

    protected override void OnStart()
    {
        if (DataManager.HasInstance)
        {
            maxHealth = DataManager.Instance.globalConfig.playerMaxHealth;
        }

        currentHealth = maxHealth;
        ragdoll = GetComponent<Ragdoll>();
        activeWeapon = GetComponent<ActiveWeapon>();
        aiming = GetComponent<CharacterAiming>();
    }

    protected override void OnDamage(Vector3 direction, Rigidbody rigidBody)
    {
        if (postProcessing.profile.TryGet(out Vignette vignette))
        {
            float percent = 1.0f - (currentHealth / maxHealth);
            vignette.intensity.value = percent * maxValue;
        }
    }

    protected override void OnDeath(Vector3 direction, Rigidbody ridigBody)
    {
        RaycastWeapon weapon = activeWeapon.GetActiveWeapon();
        ragdoll.ActiveRagdoll();
        direction.y = 1f;
        ragdoll.ApplyForce(direction, ridigBody);
        if (weapon)
        {
            activeWeapon.DropWeapon((int)activeWeapon.GetActiveWeapon().weaponSlot);
        }
        aiming.enabled = false;
        if (CameraManager.HasInstance)
        {
            CameraManager.Instance.EnableKillCam();
        }
        if (postProcessing.profile.TryGet(out Vignette vignette))
        {
            vignette.intensity.value = 0f;
        }
    }
}