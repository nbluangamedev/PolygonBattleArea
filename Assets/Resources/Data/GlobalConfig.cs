using UnityEngine;

[CreateAssetMenu()]
public class GlobalConfig : ScriptableObject
{
    [Header("AI")]
    public float maxTime = 1.0f;
    public float maxDistance = 1.0f;
    public float maxHealth = 100f;
    public float blinkDuration = 0.1f;
    public float dieForce = 10.0f;
    public float maxSight = 5.0f;

    [Header("Player")]
    public float jumpHeight = 3f;
    public float gravity = 20f;
    public float stepDown = 0.1f;
    public float airControl = 2.5f;
    public float jumpDamp = 0.5f;
    public float groundSpeed = 1.2f;
    public float pushPower = 2f;
    public float turnSpeed = 15f;
    public float defaultRecoil = 1f;
    public float aimRecoil = 0.3f;
    public float timeDestroyDroppedMagazine = 5f;
    public float maxCroissHairTargetDistance = 100f;
    public int amountInPool = 30;
}