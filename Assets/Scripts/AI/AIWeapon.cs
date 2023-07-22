using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWeapon : MonoBehaviour
{
    RaycastWeapon currentWeapon;
    Animator animator;
    MeshSocketController socketController;
    WeaponIK weaponIK;
    Transform currentTarget;

    private void Start()
    {
        animator = GetComponent<Animator>();
        socketController = GetComponent<MeshSocketController>();
        weaponIK = GetComponent<WeaponIK>();
    }

    public void Equip(RaycastWeapon weapon)
    {
        currentWeapon = weapon;
        socketController.Attach(weapon.transform, SocketID.Spine);
    }

    public void ActivateWeapon()
    {
        StartCoroutine(EquipWeapon());
    }

    IEnumerator EquipWeapon()
    {
        animator.SetBool("equip", true);
        yield return new WaitForSeconds(.5f);
        while (animator.GetCurrentAnimatorStateInfo(1).normalizedTime < 1.0f)
        {
            yield return null;
        }
        weaponIK.SetAimTransform(currentWeapon.raycastOrigin);
    }

    public void DropWeapon()
    {
        if (currentWeapon)
        {
            currentWeapon.transform.SetParent(null);
            currentWeapon.gameObject.GetComponent<BoxCollider>().enabled = true;
            currentWeapon.gameObject.AddComponent<Rigidbody>();
            currentWeapon = null;
        }
    }

    public bool HasWeapon()
    {
        return currentWeapon != null;
    }

    public void OnAnimationEvent(string eventName)
    {
        if(eventName == "equipWeapon")
        {
            socketController.Attach(currentWeapon.transform, SocketID.RightHand);
        }
    }

    public void SetTarget(Transform target)
    {
        weaponIK.SetTargetTransform(target);
        currentTarget = target;
    }
}
