using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    private int ammoAmount;

    private void Start()
    {
        if (DataManager.HasInstance)
        {
            ammoAmount = DataManager.Instance.globalConfig.ammoAmount;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ActiveWeapon playerWeapon = other.GetComponent<ActiveWeapon>();
        if (playerWeapon)
        {
            playerWeapon.RefillAmmo(ammoAmount);
            Destroy(gameObject);
        }

        AIWeapon aiWeapon = other.GetComponent<AIWeapon>();
        if (aiWeapon)
        {
            aiWeapon.RefillAmmo(ammoAmount);
            Destroy(gameObject);
        }
    }
}