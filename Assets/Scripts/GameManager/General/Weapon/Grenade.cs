using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float delayExplode = 3f;
    public float explosionRadius = 5f;
    public float explosionForce = 100f;
    public GameObject explosionEffectPrefabs;

    [SerializeField]
    private Collider[] colliders = new Collider[20];
    private float cooldown;
    private bool hasExploded = false;


    private void Start()
    {
        cooldown = delayExplode;
    }

    private void Update()
    {
        cooldown -= Time.deltaTime;
        Debug.Log("cooldown " + cooldown);
        if (cooldown <= 0)
        {
            Explode();
            hasExploded = true;
        }
    }

    private void Explode()
    {
        if (!hasExploded)
        {
            //show effect
            Instantiate(explosionEffectPrefabs, transform.position, transform.rotation).transform.SetParent(transform);

            //Get nearby object
            colliders = Physics.OverlapSphere(transform.position, explosionRadius);            
            foreach (Collider objectNearby in colliders)
            {
                //Add force
                Rigidbody rb = objectNearby.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                }

                //take damage
            }

            //remove grenade
            Invoke(nameof(DestroyGrenade), 1f);
        }
    }

    private void DestroyGrenade()
    {
        Destroy(gameObject);
    }

    //WeaponSlot weaponSlot;

    //public Transform camLookAt;
    //public Transform attackPoint;
    //public GameObject grenadePrefab;

    //public int totalGrenade;
    //public float grenadeCooldown;

    //public float throwForce;
    //public float throwUpwardForce;

    //bool readyThrow;

    //private void Start()
    //{
    //    readyThrow = true;
    //}

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Mouse0) && readyThrow && totalGrenade > 0)
    //    {
    //        ThrowGrenade();
    //    }
    //}

    //private void ThrowGrenade()
    //{
    //    readyThrow = false;

    //    GameObject grenade = Instantiate(grenadePrefab, attackPoint.position, camLookAt.rotation);

    //    Rigidbody grenadeRigid = grenade.GetComponent<Rigidbody>();

    //    Vector3 forceToAdd = camLookAt.transform.forward * throwForce + transform.up * throwUpwardForce;

    //    grenadeRigid.AddForce(forceToAdd, ForceMode.Impulse);

    //    totalGrenade--;

    //    //implement throw cooldown
    //    Invoke(nameof(ResetThrow), grenadeCooldown);
    //}

    //private void ResetThrow()
    //{
    //    readyThrow = true;
    //}
}