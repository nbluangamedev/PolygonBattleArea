using UnityEngine;

public class Health : MonoBehaviour
{
    protected float maxHealth;
    protected float currentHealth;
    protected float maxAmor;
    protected float currentAmor;
    public float CurrentHealth => currentHealth;
    public float CurrentAmor => currentAmor;

    private float lowHealth;

    private void Start()
    {
        if (DataManager.HasInstance)
        {
            lowHealth = DataManager.Instance.globalConfig.lowHealth;
        }
        Setup("HitBox");
        OnStart();
    }

    public void TakeDamage(float amount, Vector3 direction, Rigidbody rigidbody)
    {
        if (currentAmor > 0)
        {
            int takeHealthOrAmor = Mathf.RoundToInt(Random.Range(0, 2));
            if (takeHealthOrAmor == 0)
            {
                currentHealth -= amount;
            }
            else
            {
                currentAmor -= amount;
            }
        }
        else
        {
            currentHealth -= amount;
        }

        if (IsDead())
        {
            currentHealth = Mathf.Max(currentHealth, 0);
            Die(direction, rigidbody);
            return;
        }

        OnDamage(direction, rigidbody);
    }

    public void Heal(float amount)
    {
        if (currentHealth >= maxHealth)
        {
            currentAmor += amount;
            currentAmor = Mathf.Min(currentAmor, maxAmor);
        }
        else
        {
            currentHealth += amount;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
        }

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

    private void Setup(string layerMask)
    {
        Rigidbody[] rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rigidBody in rigidBodies)
        {
            rigidBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            HitBox hitBox = rigidBody.gameObject.AddComponent<HitBox>();
            hitBox.health = this;
            hitBox.rb = rigidBody;      //???
            if (hitBox.gameObject != gameObject)
            {
                hitBox.gameObject.layer = LayerMask.NameToLayer(layerMask);
            }
        }
    }

    protected void Die(Vector3 direction, Rigidbody rigidbody)
    {
        Setup("Character");
        OnDeath(direction, rigidbody);
    }

    protected virtual void OnStart() { }

    protected virtual void OnDeath(Vector3 direction, Rigidbody ridigBody) { }

    protected virtual void OnDamage(Vector3 direction, Rigidbody rigidBody) { }

    protected virtual void OnHeal(float amount) { }
}