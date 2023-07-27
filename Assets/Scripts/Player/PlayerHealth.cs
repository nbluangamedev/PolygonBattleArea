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
    CameraManager cameraManager;

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
        cameraManager = FindObjectOfType<CameraManager>();
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
        ragdoll.ActiveRagdoll();
        direction.y = 1f;
        ragdoll.ApplyForce(direction, ridigBody);
        activeWeapon.DropWeapon();
        aiming.enabled = false;
        CameraManager.Instance.EnableKillCam();
    }
}