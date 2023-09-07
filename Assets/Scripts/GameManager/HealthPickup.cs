using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public float healthAmount;

    private void Start()
    {
        if (DataManager.HasInstance)
        {
            healthAmount = DataManager.Instance.globalConfig.healthPickupAmount;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponent<Health>();
        if (health)
        {
            health.Heal(healthAmount);
            if(AudioManager.HasInstance)
            {
                AudioManager.Instance.PlaySE(AUDIO.SE_HEALTHPICKUP);
            }
            Destroy(gameObject);
        }
    }
}