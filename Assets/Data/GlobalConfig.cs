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
    public float dieForce = 10.0f;
    public float maxSight = 5.0f;
}