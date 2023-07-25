using UnityEngine;

public class Health : MonoBehaviour
{
    protected float maxHealth;
    protected float currentHealth;

    void Start()
    {
        Setup();
        OnStart();
    }

    private void Setup()
    {
        var rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rigidBody in rigidBodies)
        {
            HitBox hitBox = rigidBody.gameObject.AddComponent<HitBox>();
            hitBox.health = this;
            hitBox.rb = rigidBody;
            if (hitBox.gameObject != gameObject)
            {
                hitBox.gameObject.layer = LayerMask.NameToLayer("HitBox");
            }
        }
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    public void TakeDamage(float amount, Vector3 direction, Rigidbody rigidbody)
    {
        currentHealth -= amount;

        OnDamage(direction, rigidbody);

        if (currentHealth <= 0)
        {
            Die(direction, rigidbody);
        }
    }

    private void Die(Vector3 direction, Rigidbody rigidbody)
    {
        OnDeath(direction, rigidbody);
    }

    protected virtual void OnStart()
    {

    }

    protected virtual void OnDeath(Vector3 direction, Rigidbody ridigBody)
    {

    }

    protected virtual void OnDamage(Vector3 direction, Rigidbody rigidBody)
    {

    }
}