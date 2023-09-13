using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    private int numberEnemySpawn;
    [SerializeField]
    private EnemySpawn[] enemySpawns;

    //check enemy alive
    [SerializeField]
    private Collider[] colliders = new Collider[50];
    [SerializeField]
    private List<GameObject> agents = new();
    private int count;
    private float distance = 500f;
    [SerializeField]
    private LayerMask sensorLayer;

    private void Awake()
    {
        enemySpawns = GetComponentsInChildren<EnemySpawn>();

        sensorLayer = LayerMask.GetMask("Character");
    }

    void Start()
    {
        if (GameManager.HasInstance)
        {
            numberEnemySpawn = GameManager.Instance.enemySpawn;
        }
        int numberSpawn = numberEnemySpawn / 3;
        foreach (EnemySpawn enemySpawn in enemySpawns)
        {
            enemySpawn.numberEnemySpawn = numberSpawn;
        }
    }

    private void Update()
    {
        //check enemy death
        if (GameManager.HasInstance)
        {
            if (GameManager.Instance.EnemyCount > 0)
            {
                Scan();
                if (agents.IsNullOrEmpty())
                {
                    foreach (EnemySpawn enemySpawn in enemySpawns)
                    {
                        float timeSpawn = enemySpawn.timeToSpawn;
                        if (enemySpawn.timerSpawn < timeSpawn)
                        {
                            enemySpawn.timerSpawn = timeSpawn;
                        }
                    }
                }
            }

        }
    }

    private void Scan()
    {
        count = Physics.OverlapSphereNonAlloc(transform.position, distance, colliders, sensorLayer, QueryTriggerInteraction.Collide);
        agents.Clear();
        for (int i = 0; i < count; i++)
        {
            GameObject obj = colliders[i].gameObject;
            if (obj.CompareTag("AIAgent"))
            {
                agents.Add(obj);
            }
        }
    }
}
