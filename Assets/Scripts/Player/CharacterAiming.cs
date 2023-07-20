using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterAiming : MonoBehaviour
{
    public float turnSpeed = 15;
    public Transform cameraLookAt;
    public Cinemachine.AxisState xAxis;
    public Cinemachine.AxisState yAxis;
    public bool isAiming = false;

    Camera mainCamera;
    Animator animator;
    ActiveWeapon activeWeapon;

    int isAimingParameter = Animator.StringToHash("isAiming");

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
        activeWeapon = GetComponent<ActiveWeapon>();
    }

    private void Update()
    {
        var weapon = activeWeapon.GetActiveWeapon();
        bool canAim = !activeWeapon.isHolstered && !activeWeapon.weaponReload.isReloading;

        if (weapon && canAim)
        {
            if (Input.GetMouseButtonDown(1))
            {
                isAiming = !isAiming;
                animator.SetBool(isAimingParameter, isAiming);
            }

            weapon.recoil.recoilModifier = isAiming ? 0.3f : 1.0f;
        }
    }

    void FixedUpdate()
    {
        xAxis.Update(Time.fixedDeltaTime);
        yAxis.Update(Time.fixedDeltaTime);

        cameraLookAt.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);

        float yawCamera = mainCamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), turnSpeed * Time.fixedDeltaTime);
    }
}