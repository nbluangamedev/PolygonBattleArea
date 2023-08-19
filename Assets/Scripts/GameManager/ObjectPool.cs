using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : BaseManager<ObjectPool>
{
    [HideInInspector] public List<Bullet> pooledBulletObjects;
    [HideInInspector] public List<EnemyBullet> pooledEnemyBulletObjects;
    [HideInInspector] public List<GameObject> pooledEnemyObjects;

    public Bullet bulletToPool;
    public EnemyBullet bulletEnemyToPool;
    public GameObject enemyToPool;

    private int amountBulletInPool;
    private int amountEnemyBulletInPool;
    private int amountEnemyInPool;

    private void Start()
    {
        pooledBulletObjects = new List<Bullet>();
        pooledEnemyBulletObjects = new List<EnemyBullet>();
        //pooledEnemyObjects = new List<GameObject>();

        Bullet tmpBullet;
        EnemyBullet tmpEnemyBullet;
        //GameObject tmpEnemy;

        if (DataManager.HasInstance)
        {
            amountBulletInPool = DataManager.Instance.globalConfig.amountBulletInPool;
            amountEnemyBulletInPool = DataManager.Instance.globalConfig.amountEnemyBulletInPool;
            //amountEnemyInPool = DataManager.Instance.globalConfig.amountEnemyInPool;
        }

        for (int i = 0; i < amountBulletInPool; i++)
        {
            tmpBullet = Instantiate(bulletToPool, this.transform, true);
            tmpBullet.Deactive();
            pooledBulletObjects.Add(tmpBullet);
        }

        for (int i = 0; i < amountEnemyBulletInPool; i++)
        {
            tmpEnemyBullet = Instantiate(bulletEnemyToPool, this.transform, true);
            tmpEnemyBullet.Deactive();
            pooledEnemyBulletObjects.Add(tmpEnemyBullet);
        }

        //for (int i = 0; i < amountEnemyInPool; i++)
        //{
        //    tmpEnemy = Instantiate(enemyToPool, this.transform, true);
        //    tmpEnemy.SetActive(false);
        //    pooledEnemyObjects.Add(tmpEnemy);
        //}
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

    public EnemyBullet GetPoolEnemyBulletObject()
    {
        for (int i = 0; i < amountBulletInPool; i++)
        {
            if (!pooledBulletObjects[i].IsActive)
            {
                return pooledEnemyBulletObjects[i];
            }
        }
        return null;
    }

    //public GameObject GetPoolEnemyObject()
    //{
    //    for (int i = 0; i < amountEnemyInPool; i++)
    //    {
    //        if (!pooledEnemyObjects[i].active)
    //        {
    //            return pooledEnemyObjects[i];
    //        }
    //    }
    //    return null;
    //}
}