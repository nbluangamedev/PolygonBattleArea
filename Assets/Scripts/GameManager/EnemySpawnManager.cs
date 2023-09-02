using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    private int numberEnemySpawn;
    private EnemySpawn[] enemySpawns;

    private void Awake()
    {
        enemySpawns = GetComponentsInChildren<EnemySpawn>();
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
}
