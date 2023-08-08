using UnityEngine;

public class Health : MonoBehaviour
{
    protected float maxHealth;
    protected float currentHealth;

    private void Start()
    {
        Setup();
        OnStart();
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

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    private void Setup()
    {
        Rigidbody[] rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rigidBody in rigidBodies)
        {
            rigidBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            HitBox hitBox = rigidBody.gameObject.AddComponent<HitBox>();
            hitBox.health = this;
            hitBox.rb = rigidBody;
            if (hitBox.gameObject != gameObject)
            {
                hitBox.gameObject.layer = LayerMask.NameToLayer("HitBox");
            }
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