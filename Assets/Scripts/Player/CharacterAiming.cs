using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterAiming : MonoBehaviour
{
    public Transform cameraLookAt;
    public Cinemachine.AxisState xAxis;
    public Cinemachine.AxisState yAxis;
    public bool isAiming = false;

    public GameObject scopeOverlay;
    public CinemachineVirtualCamera weaponCamera;
    public LayerMask defaultMask;
    public LayerMask weaponMask;
    public float scopedFOV = 15f;

    private float normalFOV;
    private float turnSpeed;
    private float defaultRecoil;
    private float aimRecoil;

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
        if (DataManager.HasInstance)
        {
            turnSpeed = DataManager.Instance.globalConfig.turnSpeed;
            defaultRecoil = DataManager.Instance.globalConfig.defaultRecoil;
            aimRecoil = DataManager.Instance.globalConfig.aimRecoil;
        }
    }

    private void Update()
    {
        var weapon = activeWeapon.GetActiveWeapon();
        bool canAim = !activeWeapon.isHolstered && !activeWeapon.weaponReload.isReloading;

        if (isAiming)
        {
            if (Input.GetKeyDown(KeyCode.R) || weapon.ammoCount <= 0)
            {
                UnScopeAndAim(weapon);
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                UnScopeAndAim(weapon);
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (activeWeapon.isChangingWeapon)
                {
                    UnScopeAndAim(weapon);
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (activeWeapon.isChangingWeapon)
                {
                    UnScopeAndAim(weapon);
                }
            }
        }

        if (weapon && canAim)
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (weapon.weaponName.Equals("Sniper"))
                {
                    isAiming = !isAiming;

                    if (isAiming)
                    {
                        StartCoroutine(OnScope());
                    }
                    else
                    {
                        StartCoroutine(UnScope());
                    }
                }
                else if (weapon.weaponName.Equals("Shotgun"))
                {
                    return;
                }
                else
                {
                    UnAiming(weapon);
                }
            }
        }
    }

    private void UnScopeAndAim(RaycastWeapon weapon)
    {
        if (weapon.weaponName.Equals("Sniper"))
        {
            StartCoroutine(UnScope());
        }
        else
        {
            UnAiming(weapon);
        }
    }

    private void UnAiming(RaycastWeapon weapon)
    {
        isAiming = !isAiming;
        animator.SetBool(isAimingParameter, isAiming);
        weaponCamera.m_Lens.FieldOfView = isAiming ? 25 : 60;
        weapon.recoil.recoilModifier = isAiming ? aimRecoil : defaultRecoil;
    }

    void FixedUpdate()
    {
        xAxis.Update(Time.fixedDeltaTime);
        yAxis.Update(Time.fixedDeltaTime);

        cameraLookAt.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);

        float yawCamera = mainCamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), turnSpeed * Time.fixedDeltaTime);
    }

    IEnumerator UnScope()
    {
        yield return new WaitForSeconds(0.1f);

        scopeOverlay.SetActive(false);

        mainCamera.cullingMask = defaultMask;

        weaponCamera.m_Lens.FieldOfView = normalFOV;
    }

    IEnumerator OnScope()
    {
        yield return new WaitForSeconds(0.1f);

        scopeOverlay.SetActive(true);

        mainCamera.cullingMask = weaponMask;
        normalFOV = mainCamera.fieldOfView;

        weaponCamera.m_Lens.FieldOfView = scopedFOV;
    }
}