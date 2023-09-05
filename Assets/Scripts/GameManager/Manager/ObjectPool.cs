using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : BaseManager<ObjectPool>
{
    [HideInInspector] public List<Bullet> pooledBulletObjects;
    [HideInInspector] public List<EnemyBullet> pooledEnemyBulletObjects;

    public Bullet bulletToPool;
    public EnemyBullet bulletEnemyToPool;

    private int amountBulletInPool;
    private int amountEnemyBulletInPool;

    private void Start()
    {
        pooledBulletObjects = new List<Bullet>();
        pooledEnemyBulletObjects = new List<EnemyBullet>();

        Bullet tmpBullet;
        EnemyBullet tmpEnemyBullet;

        if (DataManager.HasInstance)
        {
            amountBulletInPool = DataManager.Instance.globalConfig.amountBulletInPool;
            amountEnemyBulletInPool = DataManager.Instance.globalConfig.amountEnemyBulletInPool;
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
}