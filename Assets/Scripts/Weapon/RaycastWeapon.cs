using System.Collections;
using UnityEngine;

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
    public Transform raycastOrigin;
    public WeaponRecoil recoil;
    public GameObject magazine;
    public float forceBullet = 2f;
    public int ammoCount;
    public int clipSize;
    public float damage = 10f;
    public GameObject BulletCasingPrefab;
    public Transform GunSlider;

    public LayerMask layerMask;
    public RuntimeAnimatorController overrideAnimator;

    private Ray ray;
    private RaycastHit hitInfo;
    private float accumulatedTime;
    private float maxLifetime = 2.0f;
    private int lossOfAccuracyPerShot;
    [SerializeField] private CharacterAiming characterAiming;

    private void Awake()
    {
        if (DataManager.HasInstance)
        {
            lossOfAccuracyPerShot = DataManager.Instance.globalConfig.lossOfAccuracyPerShot;
        }

        recoil = GetComponent<WeaponRecoil>();
        characterAiming = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterAiming>();
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

    public void UpdateWeapon(float deltaTime, Vector3 target)
    {
        if (isFiring)
        {
            UpdateFiring(deltaTime, target);
        }

        accumulatedTime += deltaTime;

        UpdateBullets(deltaTime);
    }

    public void UpdateFiring(float deltaTime, Vector3 target)
    {
        float fireInterval = 1.0f / fireRate;
        while (accumulatedTime >= 0.0f)
        {
            FireBullet(target);
            accumulatedTime -= fireInterval;
        }
    }

    public void UpdateBullets(float deltaTime)
    {
        SimulateBullets(deltaTime);
        DestroyBullets();
    }

    private void RaycastSegment(Vector3 start, Vector3 end, Bullet bullet)
    {
        if (weaponName != "Shotgun")
        {
            Vector3 direction = end - start;
            float distance = direction.magnitude;
            ray.origin = start;
            ray.direction = direction;

            if (Physics.Raycast(ray, out hitInfo, distance, layerMask))
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
            for (int i = 0; i < 6; i++)
            {
                var BulletRotationPrecision = end;
                BulletRotationPrecision.x += Random.Range(-lossOfAccuracyPerShot, lossOfAccuracyPerShot);
                BulletRotationPrecision.y += Random.Range(-lossOfAccuracyPerShot, lossOfAccuracyPerShot);
                BulletRotationPrecision.z += Random.Range(-lossOfAccuracyPerShot, lossOfAccuracyPerShot);

                Vector3 direction = BulletRotationPrecision - start;
                float distance = direction.magnitude;
                ray.origin = start;
                ray.direction = direction;

                if (Physics.Raycast(ray, out hitInfo, distance, layerMask))
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

    private void FireBullet(Vector3 target)
    {
        Vector3 velocity = (target - raycastOrigin.position).normalized * bulletSpeed;
        Bullet bullet = ObjectPool.Instance.GetPoolObject();
        bullet.Active(raycastOrigin.position, velocity);

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

        EmitBulletCasing();

        if (this.equipWeaponBy == EquipWeaponBy.Player && this.weaponName.Equals("Sniper") && ammoCount > 0)
        {
            if (characterAiming.isAiming)
            {
                StartCoroutine(ActivateOnScope());
            }
            else recoil.rigController.Play("sniperPullBolt");
        }

        recoil.GenerateRecoil(weaponName);
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

    private Vector3 GetPosition(Bullet bullet)
    {
        //p + v*t + 0.5*g*t*t
        Vector3 gravity = Vector3.down * bulletDrop;
        return (bullet.initialPosition) + (bullet.initialVelocity * bullet.time) + (0.5f * gravity * bullet.time * bullet.time);
    }

    public void EmitBulletCasing()
    {
        //Spawn Bullet Casing
        if (BulletCasingPrefab != null)
        {
            var bulletcasing = Instantiate(BulletCasingPrefab, GunSlider.position, transform.rotation);
            bulletcasing.hideFlags = HideFlags.HideInHierarchy;
            Destroy(bulletcasing, 5f);
        }
    }

    private IEnumerator ActivateOnScope()
    {
        yield return new WaitForSeconds(.1f);
        recoil.rigController.Play("sniperPullBolt");
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.BroadCast(ListenType.SCOPE, false);
            ListenerManager.Instance.BroadCast(ListenType.ACTIVECROSSHAIR, false);
            characterAiming.UnScopeAndAim(this);
            characterAiming.isAiming = true;
        }

        while (recoil.rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        } 
        yield return new WaitForSeconds(1f);

        if (characterAiming.isAiming)
        {
            StartCoroutine(characterAiming.OnScope());
        }
    }
}