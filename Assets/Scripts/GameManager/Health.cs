using UnityEngine;

public class Health : MonoBehaviour
{
    protected float maxHealth;
    protected float currentHealth;
    public float CurrentHealth => currentHealth;
    private float lowHealth;

    private void Start()
    {
        if (DataManager.HasInstance)
        {
            lowHealth = DataManager.Instance.globalConfig.lowHealth;
        }
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

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        
        OnHeal(amount);
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    public bool IsLowHealth()
    {
        return currentHealth < lowHealth;
    }

    private void Setup()
    {
        Rigidbody[] rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rigidBody in rigidBodies)
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

    protected void Die(Vector3 direction, Rigidbody rigidbody)
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

    protected virtual void OnHeal(float amount)
    {

    }
}