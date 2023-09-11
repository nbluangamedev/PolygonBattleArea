using UnityEngine;
using Cinemachine;

public class WeaponRecoil : MonoBehaviour
{
    [HideInInspector] public CharacterAiming characterAiming;
    [HideInInspector] public CinemachineImpulseSource cameraShake;
    [HideInInspector] public Animator rigController;
    [HideInInspector] public float recoilModifier = 1.0f;

    public Vector2[] recoilPattern;

    private float duration;
    private float verticalRecoil;
    private float horizontalRecoil;
    private float time;
    private int index;
    private int recoilLayerIndex = -1;

    private void Awake()
    {
        cameraShake = GetComponent<CinemachineImpulseSource>();
    }

    private void Start()
    {
        if (DataManager.HasInstance)
        {
            duration = DataManager.Instance.globalConfig.duration;
        }

        if (rigController)
        {
            recoilLayerIndex = rigController.GetLayerIndex("Recoil Layer");
        }
    }

    private void Update()
    {
        if (rigController && time > 0)
        {
            characterAiming.yAxis.Value -= (((verticalRecoil / 10) * Time.deltaTime) / duration) * recoilModifier;
            characterAiming.xAxis.Value -= (((horizontalRecoil / 10) * Time.deltaTime) / duration) * recoilModifier;
            time -= Time.deltaTime;
        }
    }

    public void GenerateRecoil(string weaponName)
    {
        if (rigController)
        {
            time = duration;

            cameraShake.GenerateImpulse(Camera.main.transform.forward);

            horizontalRecoil = recoilPattern[index].x;
            verticalRecoil = recoilPattern[index].y;

            index = NextIndex(index);

            rigController.Play("weapon_Recoil_" + weaponName, recoilLayerIndex, 0f);
        }        
    }

    public void Reset()
    {
        index = 0;
    }

    private int NextIndex(int index)
    {
        return (index + 1) % recoilPattern.Length;
    }
}