using UnityEngine;

[CreateAssetMenu()]
public class GlobalConfig : ScriptableObject
{
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

    [Header("Character Selection")]
    public float speedRotateCharacter = 5f;

    //---------------------------WEAPON--------------------------//
    [Header("WEAPON")]
    public int ammoPickupAmount = 30;

    [Header("Weapon Recoil")]
    public float duration;

    [Header("Bullet")]
    public int amountBulletInPool = 30;
    public int amountEnemyBulletInPool = 30;
    public int amountEnemyInPool = 30;

    //----------------------------UI---------------------------//
    [Header("UI")]
    [Header("Overlap")]
    public float loadingOverLapTime = 2.0f;
    public float loadingOverLapTimeCharacterSelection = 1.0f;

    //----------------------------AI---------------------------//
    [Header("AI State")]
    [Header("Chase")]
    public float maxTime = 1.0f;
    public float maxDistance = 1.0f;
    public float closeRange = 7.0f;

    [Header("Death")]
    public float enemyDieForce = .5f;

    [Header("Find weapon")]
    public float findWeaponSpeed = 7f;

    [Header("Attack")]
    public float attackStoppingDistance = 15f;
    public float attackMoveSpeed = 3.0f;
    public float attackRadius = 20f;

    [Header("Find target")]
    public float findTargetSpeed = 5f;
    public float findTargetAcceleration = 20f;
    public float findTargetTurnSpeed = 720f;
    public float findTargetWaitTime = 1f;
    public float findTargetRadius = 20f;

    [Header("Patrol")]
    public float patrolRadius = 10f;
    public float patrolSpeed = 7.0f;
    public float patrolAcceleration = 8f;
    public float patrolTurnSpeed = 520f;
    public float patrolWaitTime = 1f;

    [Header("Flee")]
    public float fleeRange = 30f;

    [Header("Health")]
    public float aiMaxHealth = 100f;
    public float blinkDuration = 0.1f;
    public float timeDestroyAI = 2f;
    public float lowHealth = 40f;
    public float healthPickupAmount = 50f;

}