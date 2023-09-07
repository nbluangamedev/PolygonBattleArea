using Cinemachine;
using System.Collections;
using UnityEngine;

public class CharacterAiming : MonoBehaviour
{
    public AxisState xAxis;
    public AxisState yAxis;
    public bool isAiming = false;
    public Transform cameraLookAt;

    private CinemachineVirtualCamera weaponCamera;

    [SerializeField]
    private Camera mainCamera;
    private Animator animator;
    private ActiveWeapon activeWeapon;
    private LayerMask defaultMask;
    private LayerMask weaponMask;
    private float scopedFOV = 15f;
    private float aimFOV = 25f;
    private float normalFOV;
    private float turnSpeed;
    private float defaultRecoil;
    private float aimRecoil;
    private int isAimingParameter = Animator.StringToHash("isAiming");

    private void Start()
    {
        if (CameraManager.HasInstance)
        {
            weaponCamera = CameraManager.Instance.weaponCamera.GetComponent<CinemachineVirtualCamera>();
        }

        if (DataManager.HasInstance)
        {
            turnSpeed = DataManager.Instance.globalConfig.turnSpeed;
            defaultRecoil = DataManager.Instance.globalConfig.defaultRecoil;
            aimRecoil = DataManager.Instance.globalConfig.aimRecoil;
            scopedFOV = DataManager.Instance.globalConfig.scopedFOV;
            normalFOV = DataManager.Instance.globalConfig.normalFOV;
            defaultMask = DataManager.Instance.globalConfig.defaultMask;
            weaponMask = DataManager.Instance.globalConfig.weaponMask;
        }
        if (GameManager.HasInstance)
        {
            GameManager.Instance.LockCursor();
        }

        mainCamera = Camera.main;
        //mainCamera = gameObject.transform.Find("MainCamera").GetComponent<Camera>();
        animator = GetComponent<Animator>();
        activeWeapon = GetComponent<ActiveWeapon>();

        weaponCamera.Follow = cameraLookAt;
        weaponCamera.LookAt = cameraLookAt;
    }

    private void Update()
    {
        var weapon = activeWeapon.GetActiveWeapon();
        if (weapon)
        {
            bool canAim = !activeWeapon.isHolstered && !activeWeapon.weaponReload.isReloading && weapon.ammoCount >= 1;

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
                    isAiming = !isAiming;
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
                        if (isAiming)
                        {
                            Aiming(weapon);
                        }
                        else
                        {
                            UnAiming(weapon);
                        }
                    }
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
        if (weapon.weaponName.Equals("Sniper"))
        {
            StartCoroutine(UnScope());
        }
        UnAiming(weapon);
    }

    private void UnAiming(RaycastWeapon weapon)
    {
        animator.SetBool(isAimingParameter, false);
        weaponCamera.m_Lens.FieldOfView = normalFOV;
        weapon.recoil.recoilModifier = defaultRecoil;
    }

    private void Aiming(RaycastWeapon weapon)
    {
        animator.SetBool(isAimingParameter, true);
        weaponCamera.m_Lens.FieldOfView = aimFOV;
        weapon.recoil.recoilModifier = aimRecoil;
    }

    public IEnumerator UnScope()
    {
        isAiming = false;
        yield return new WaitForSeconds(0.1f);
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.BroadCast(ListenType.SCOPE, false);
            ListenerManager.Instance.BroadCast(ListenType.ACTIVECROSSHAIR, false);
        }
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_SNIPERSCOPE);
        }
        mainCamera.cullingMask = defaultMask;
        weaponCamera.m_Lens.FieldOfView = normalFOV;
    }

    public IEnumerator OnScope()
    {
        isAiming = true;
        yield return new WaitForSeconds(0.1f);
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.BroadCast(ListenType.SCOPE, true);
            ListenerManager.Instance.BroadCast(ListenType.ACTIVECROSSHAIR, false);
        }
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_SNIPERSCOPE);
        }
        mainCamera.cullingMask = weaponMask;
        weaponCamera.m_Lens.FieldOfView = scopedFOV;
    }
}