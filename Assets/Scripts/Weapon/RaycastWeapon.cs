using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class RaycastWeapon : MonoBehaviour
{
    public EquipWeaponBy equipWeaponBy;
    public WeaponSlot weaponSlot;

    public bool isFiring = false;
    public int fireRate = 15;
    public float bulletSpeed = 1000.0f;
    public float bulletDrop = 0.0f;
    public ParticleSystem[] muzzleFlash;
    public ParticleSystem hitEffect;
    public TrailRenderer tracerEffect;
    public string weaponName;
    public LayerMask layerMask;

    public Transform raycastOrigin;
    public WeaponRecoil recoil;
    public GameObject magazine;
    public float forceBullet = 2f;
    public int ammoCount;
    public int clipSize;
    public float damage = 10f;
    public RuntimeAnimatorController overrideAnimator;

    Ray ray;
    RaycastHit hitInfo;
    float accumulatedTime;
    float maxLifetime = 2.0f;

    private void Awake()
    {
        recoil = GetComponent<WeaponRecoil>();
    }

    public void StartFiring()
    {
        isFiring = true;
        if (accumulatedTime > 0f)
        {
            accumulatedTime = 0f;
        }
        recoil.Reset();
    }

    public void StopFiring()
    {
        isFiring = false;
    }

    public void UpdateFiring(float deltaTime, Vector3 target)
    {
        //accumulatedTime += deltaTime;
        float fireInterval = 1.0f / fireRate;
        while (accumulatedTime >= 0.0f)
        {
            FireBullet(target);
            accumulatedTime -= fireInterval;
        }
    }

    public void UpdateWeapon(float deltaTime, Vector3 target)
    {
        if (isFiring)
        {
            UpdateFiring(deltaTime, target);
        }

        accumulatedTime += deltaTime;

        UpdateBullets(deltaTime);
    }

    private void FireBullet(Vector3 target)
    {
        if (ammoCount <= 0)
        {
            return;
        }
        ammoCount--;

        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.BroadCast(ListenType.UPDATE_AMMO, this);
        }

        foreach (var p in muzzleFlash)
        {
            p.Emit(1);
        }

        Vector3 velocity = (target - raycastOrigin.position).normalized * bulletSpeed;
        var bullet = ObjectPool.Instance.GetPoolObject();
        bullet.Active(raycastOrigin.position, velocity);

        recoil.GenerateRecoil(weaponName);
    }

    public void UpdateBullets(float deltaTime)
    {
        SimulateBullets(deltaTime);
        DestroyBullets();
    }

    private void SimulateBullets(float deltaTime)
    {
        ObjectPool.Instance.pooledObjects.ForEach(bullet =>
        {
            Vector3 p0 = GetPosition(bullet);
            bullet.time += deltaTime;
            Vector3 p1 = GetPosition(bullet);
            RaycastSegment(p0, p1, bullet);
        });
    }

    private void RaycastSegment(Vector3 start, Vector3 end, Bullet bullet)
    {
        if(weaponName != "Shotgun")
        {
            Vector3 direction = end - start;
            float distance = direction.magnitude;
            ray.origin = start;
            ray.direction = direction;

            if (Physics.Raycast(ray, out hitInfo, distance))
            {
                hitEffect.transform.position = hitInfo.point;
                hitEffect.transform.forward = hitInfo.normal;
                hitEffect.Emit(1);

                bullet.transform.position = hitInfo.point;
                bullet.time = maxLifetime;

                var rb = hitInfo.collider.GetComponent<Rigidbody>();
                if (rb)
                {
                    rb.AddForceAtPosition(ray.direction * forceBullet, hitInfo.point, ForceMode.Impulse);
                }

                var hitBox = hitInfo.collider.GetComponent<HitBox>();
                if (hitBox)
                {
                    hitBox.OnHit(this, ray.direction);
                }
            }
            else
            {
                bullet.transform.position = end;
            }
        }
        else
        {
            for(int i = 0; i < 5; i++)
            {
                var BulletRotationPrecision = end;
                BulletRotationPrecision.x += Random.Range(-1, 1);
                BulletRotationPrecision.y += Random.Range(-1, 1);
                BulletRotationPrecision.z += Random.Range(-1, 1);
                //ShotErrorProbability = ShotErrorProbability + 5 * LossOfAccuracyPerShot;

                Vector3 direction = BulletRotationPrecision - start;
                float distance = direction.magnitude;
                ray.origin = start;
                ray.direction = direction;

                if (Physics.Raycast(ray, out hitInfo, distance))
                {
                    hitEffect.transform.position = hitInfo.point;
                    hitEffect.transform.forward = hitInfo.normal;
                    hitEffect.Emit(1);

                    bullet.transform.position = hitInfo.point;
                    bullet.time = maxLifetime;

                    var rb = hitInfo.collider.GetComponent<Rigidbody>();
                    if (rb)
                    {
                        rb.AddForceAtPosition(ray.direction * forceBullet, hitInfo.point, ForceMode.Impulse);
                    }

                    var hitBox = hitInfo.collider.GetComponent<HitBox>();
                    if (hitBox)
                    {
                        hitBox.OnHit(this, ray.direction);
                    }
                }
                else
                {
                    bullet.transform.position = end;
                }
            }
        }
        
    }

    private void DestroyBullets()
    {
        foreach (Bullet bullet in ObjectPool.Instance.pooledObjects)
        {
            if (bullet.time >= maxLifetime)
            {
                bullet.Deactive();
            }
        }
    }

    Vector3 GetPosition(Bullet bullet)
    {
        //p + v*t + 0.5*g*t*t
        Vector3 gravity = Vector3.down * bulletDrop;
        return (bullet.initialPosition) + (bullet.initialVelocity * bullet.time) + (0.5f * gravity * bullet.time * bullet.time);
    }
}