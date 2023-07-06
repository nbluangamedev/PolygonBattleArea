using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterAiming : MonoBehaviour
{
    public float turnSpeed = 15;
    public float aimDuration = 0.3f;
    public Transform cameraLookAt;
    public Cinemachine.AxisState xAxis;
    public Cinemachine.AxisState yAxis;
    public bool isAiming;

    Camera mainCamera;
    RaycastWeapon weapon;
    Animator animator;
    ActiveWeapon activeWeapon;

    int isAimingParameter = Animator.StringToHash("isAiming");

    void Start()
    {
        mainCamera = Camera.main;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        weapon = GetComponentInChildren<RaycastWeapon>();
        animator = GetComponent<Animator>();
        activeWeapon = GetComponent<ActiveWeapon>();
    }

    private void Update()
    {
        isAiming = Input.GetMouseButton(1);
        animator.SetBool(isAimingParameter, isAiming);

        var weapon = activeWeapon.GetActiveWeapon();
        if (weapon)
        {
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

    private void LateUpdate()
    {
        if (weapon)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                weapon.StartFiring();
            }

            if (weapon.isFiring)
            {
                weapon.UpdateFiring(Time.deltaTime);
            }

            weapon.UpdateBullets(Time.deltaTime);

            if (Input.GetButtonUp("Fire1"))
            {
                weapon.StopFiring();
            }
        }
    }
}