using UnityEngine;

public class GrenadeThrow : MonoBehaviour
{
    public float throwForce = 20f;
    public float throwUpwardForce = 10f;
    public GameObject grenadePrefab;
    public Transform crossHairTarget;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ThrowGrenade();
        }
    }

    private void ThrowGrenade()
    {
        GameObject grenade = Instantiate(grenadePrefab, transform.position, transform.rotation);
        Rigidbody rigidbody = grenade.GetComponent<Rigidbody>();
        rigidbody.AddForce(transform.forward * throwForce + transform.up * throwUpwardForce);
    }
}
