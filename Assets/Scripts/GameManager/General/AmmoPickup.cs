using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ActiveWeapon playerWeapon = other.GetComponent<ActiveWeapon>();
        if (playerWeapon)
        {
            RaycastWeapon raycastWeapon = playerWeapon.GetActiveWeapon();
            if (raycastWeapon)
            {
                playerWeapon.RefillAmmo(raycastWeapon.clipSize * 2);
                Destroy(gameObject);
                if (AudioManager.HasInstance)
                {
                    AudioManager.Instance.PlaySE(AUDIO.SE_AMMOPICKUP);
                }
            }            
        }

        AIWeapon aiWeapon = other.GetComponent<AIWeapon>();
        if (aiWeapon)
        {
            RaycastWeapon raycastAIWeapon = aiWeapon.AICurrentWeapon;
            if (raycastAIWeapon)
            {
                aiWeapon.RefillAmmo(raycastAIWeapon.clipSize * 2);
                Destroy(gameObject);
                if (AudioManager.HasInstance)
                {
                    AudioManager.Instance.PlaySEAgent(AUDIO.SE_AMMOPICKUP, AudioManager.Instance.AttachSESource.volume);
                }
            }
        }
    }
}