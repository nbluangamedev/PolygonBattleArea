using Cinemachine;
using Sirenix.Utilities;
using System.Collections;
using UnityEngine;

public class CharacterAiming : MonoBehaviour
{
    public Cinemachine.AxisState xAxis;
    public Cinemachine.AxisState yAxis;
    public bool isAiming = false;
    public Transform cameraLookAt;

    private CinemachineVirtualCamera weaponCamera;
    //private GameObject scopeOverlay;

    private Camera mainCamera;
    private Animator animator;
    private ActiveWeapon activeWeapon;
    private LayerMask defaultMask;
    private LayerMask weaponMask;
    private float scopedFOV = 15f;
    private float normalFOV;
    private float turnSpeed;
    private float defaultRecoil;
    private float aimRecoil;
    private int isAimingParameter = Animator.StringToHash("isAiming");

    private void Start()
    {
        if (CameraManager.HasInstance)
        {
            weaponCamera = CameraManager.Instance.weaponCamera;
        }

        if (DataManager.HasInstance)
        {
            turnSpeed = DataManager.Instance.globalConfig.turnSpeed;
            defaultRecoil = DataManager.Instance.globalConfig.defaultRecoil;
            aimRecoil = DataManager.Instance.globalConfig.aimRecoil;
            scopedFOV = DataManager.Instance.globalConfig.scopedFOV;
            defaultMask = DataManager.Instance.globalConfig.defaultMask;
            weaponMask = DataManager.Instance.globalConfig.weaponMask;
        }

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

            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (isAiming)
                {
                    if (activeWeapon.isChangingWeapon)
                    {
                        UnScopeAndAim(weapon);
                    }
                }
            }
        }

        if (weapon && canAim)
        {
            if (Input.GetMouseButtonDown(1))
            {
                isAiming = !isAiming;
                if (weapon.weaponName.Equals("Sniper"))
                {
                    if (isAiming)
                    {
                        StartCoroutine(OnScope());
                    }
                    else
                    {
                        StartCoroutine(UnScope());
                    }
                }
                if (weapon.weaponName.Equals("Shotgun"))
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

    private void FixedUpdate()
    {
        xAxis.Update(Time.fixedDeltaTime);
        yAxis.Update(Time.fixedDeltaTime);

        cameraLookAt.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);

        float yawCamera = mainCamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), turnSpeed * Time.fixedDeltaTime);
    }

    public void UnScopeAndAim(RaycastWeapon weapon)
    {
        isAiming = false;
        normalFOV = 60f;
        if (weapon.weaponName.Equals("Sniper"))
        {            
            StartCoroutine(UnScope());
        }
        UnAiming(weapon);
    }

    private void UnAiming(RaycastWeapon weapon)
    {
        animator.SetBool(isAimingParameter, isAiming);
        weaponCamera.m_Lens.FieldOfView = isAiming ? 25 : 60;
        weapon.recoil.recoilModifier = isAiming ? aimRecoil : defaultRecoil;
    }

    private IEnumerator UnScope()
    {
        yield return new WaitForSeconds(0.1f);

        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.BroadCast(ListenType.SCOPE, isAiming);
        }

        mainCamera.cullingMask = defaultMask;

        weaponCamera.m_Lens.FieldOfView = normalFOV;
    }

    private IEnumerator OnScope()
    {
        yield return new WaitForSeconds(0.1f);

        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.BroadCast(ListenType.SCOPE, isAiming);
        }

        mainCamera.cullingMask = weaponMask;
        normalFOV = mainCamera.fieldOfView;

        weaponCamera.m_Lens.FieldOfView = scopedFOV;
    }
}