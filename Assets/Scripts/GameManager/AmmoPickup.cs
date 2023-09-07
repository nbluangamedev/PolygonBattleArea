using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ActiveWeapon playerWeapon = other.GetComponent<ActiveWeapon>();
        if (playerWeapon)
        {
            RaycastWeapon raycastWeapon = playerWeapon.GetActiveWeapon();
            playerWeapon.RefillAmmo(raycastWeapon.clipSize * 2);
            Destroy(gameObject);
            if (AudioManager.HasInstance)
            {
                //AudioManager.Instance.PlaySE(AUDIO.)
            }
        }

        AIWeapon aiWeapon = other.GetComponent<AIWeapon>();
        if (aiWeapon)
        {
            RaycastWeapon raycastWeapon = aiWeapon.AICurrentWeapon;
            aiWeapon.RefillAmmo(raycastWeapon.clipSize * 2);
            Destroy(gameObject);
            if (AudioManager.HasInstance)
            {
                //AudioManager.Instance.PlaySE(AUDIO.)
            }
        }
    }
}