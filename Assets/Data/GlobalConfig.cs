using UnityEngine;

[CreateAssetMenu()]
public class GlobalConfig : ScriptableObject
{
    //----------------------------AI---------------------------//
    [Header("AI")]
    [Header("State")]
    public float maxTime = 1.0f;
    public float maxDistance = 1.0f;
    public float enemyDieForce = .5f;
    public float stoppingDistance = 15f;
    public float closeRange = 7.0f;
    public float findWeaponSpeed = 7f;
    public float findTargetSpeed = 5f;
    public float attackMoveSpeed = 3.0f;
    public float patrolRadius = 10f;
    public float patrolSpeed = 7.0f;
    public float patrolAcceleration = 8f;
    public float patrolTurnSpeed = 520f;
    public float patrolWaitTime = 1f;
    public float fleeRange = 30f;
    public float attackRadius = 20f;
    public float findTargetAcceleration = 20f;
    public float findTargetTurnSpeed = 720f;
    public float findTargetWaitTime = 1f;
    public float findTargetRadius = 20f;


    [Header("Health")]
    public float maxHealth = 100f;
    public float blinkDuration = 0.1f;
    public float timeDestroyAI = 2f;
    public float lowHealth = 20f;

    [Header("Weapon")]
    public float inAccuracy = 0.4f;
    public float inAccuracySniper = 2f;


    //--------------------------PLAYER-------------------------//
    [Header("PLAYER")]
    [Header("Locomotion")]
    public float gravity;
    public float groundSpeed;
    public float stepDown;
    public float jumpHeight;
    public float jumpDamp;
    public float airControl;
    public float pushPower;
    public float animationSmoothTime = 0.15f;

    [Header("Aiming")]
    public float defaultRecoil = 1f;
    public float aimRecoil = 0.3f;
    public float turnSpeed = 15f;
    public float normalFOV = 15f;
    public float scopedFOV = 15f;
    public float aimFOV = 25f;
    public LayerMask defaultMask;
    public LayerMask weaponMask;
    public GameObject scopeOverlay;

    [Header("Health")]
    public float playerMaxHealth = 100f;


    //---------------------------WEAPON--------------------------//
    [Header("WEAPON")]
    public int lossOfAccuracyPerShot = 1;
    public int ammoAmount = 30;

    [Header("Weapon Recoil")]
    public float duration;

    [Header("Bullet")]
    public int amountInPool = 30;

    //----------------------------UI---------------------------//
    [Header("UI")]
    [Header("Overlap")]
    public float loadingOverLapTime = 1.0f;
}