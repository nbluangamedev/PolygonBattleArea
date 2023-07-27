using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWeapon : MonoBehaviour
{
    public float inAccurancy = 0.4f;

    RaycastWeapon currentWeapon;
    Animator animator;
    MeshSocketController socketController;
    WeaponIK weaponIK;
    Transform currentTarget;
    bool weaponActive = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        weaponIK = GetComponent<WeaponIK>();
        socketController = GetComponent<MeshSocketController>();
    }

    private void Update()
    {
        if (currentTarget && currentWeapon && weaponActive)
        {
            Vector3 target = currentTarget.position + weaponIK.targetOffset;
            target += Random.insideUnitSphere * inAccurancy;
            currentWeapon.UpdateWeapon(Time.deltaTime, target);
        }
    }

    public void SetFiring(bool enable)
    {
        if(enable)
        {
            currentWeapon.StartFiring();
        }
        else
        {
            currentWeapon.StopFiring();
        }
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
        weaponActive = true;
    }

    public void DeActivateWeapon()
    {
        SetTarget(null);
        SetFiring(false);
        StartCoroutine(Holster());
    }

    IEnumerator Holster()
    {
        weaponActive = false;
        animator.SetBool("equip", false);
        yield return new WaitForSeconds(0.5f);
        while (animator.GetCurrentAnimatorStateInfo(1).normalizedTime < 1.0f)
        {
            yield return null;
        }

        weaponIK.SetAimTransform(currentWeapon.raycastOrigin);
        //weaponActive = false;
    }

    public bool HasWeapon()
    {
        return currentWeapon != null;
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

    public void OnAnimationEvent(string eventName)
    {
        if (eventName == "equipWeapon")
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