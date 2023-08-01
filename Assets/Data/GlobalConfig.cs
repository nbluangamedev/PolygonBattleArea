using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class GlobalConfig : ScriptableObject
{
    [Header("AI")]
    public float maxTime = 1.0f;
    public float maxDistance = 1.0f;
    public float maxHealth = 100f;
    public float blinkDuration = 0.1f;
    public float enemyDieForce = .5f;
    public float maxSight = 5.0f;

    public float timeDestroyAI = 2f;

    [Header("Player")]
    public int amountInPool = 30;

    public float playerMaxHealth = 100f;
    public float turnSpeed = 15f;
    public float defaultRecoil = 1f;
    public float aimRecoil = 0.3f;

    [Header("UI")]
    public float loadingOverLapTime = 1.0f;
}