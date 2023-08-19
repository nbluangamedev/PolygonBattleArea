using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : BaseManager<ObjectPool>
{
    [HideInInspector] public List<Bullet> pooledBulletObjects;
    [HideInInspector] public List<GameObject> pooledEnemyObjects;
    public Bullet bulletToPool;
    public GameObject enemyToPool;

    private int amountBulletInPool;
    private int amountEnemyInPool;

    private void Start()
    {
        pooledBulletObjects = new List<Bullet>();
        pooledEnemyObjects = new List<GameObject>();

        Bullet tmpBullet;
        GameObject tmpEnemy;

        if (DataManager.HasInstance)
        {
            amountBulletInPool = DataManager.Instance.globalConfig.amountBulletInPool;
            amountEnemyInPool = DataManager.Instance.globalConfig.amountEnemyInPool;
        }

        for (int i = 0; i < amountBulletInPool; i++)
        {
            tmpBullet = Instantiate(bulletToPool, this.transform, true);
            tmpBullet.Deactive();
            pooledBulletObjects.Add(tmpBullet);
        }

        for (int i = 0; i < amountEnemyInPool; i++)
        {
            tmpEnemy = Instantiate(enemyToPool, this.transform, true);
            tmpEnemy.SetActive(false);
            pooledEnemyObjects.Add(tmpEnemy);
        }
    }

    public Bullet GetPoolBulletObject()
    {
        for (int i = 0; i < amountBulletInPool; i++)
        {
            if (!pooledBulletObjects[i].IsActive)
            {
                return pooledBulletObjects[i];
            }
        }
        return null;
    }

    public GameObject GetPoolEnemyObject()
    {
        for (int i = 0; i < amountEnemyInPool; i++)
        {
            if (!pooledEnemyObjects[i].active)
            {
                return pooledEnemyObjects[i];
            }
        }
        return null;
    }
}