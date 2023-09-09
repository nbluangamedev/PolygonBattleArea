using System.Collections;
using UnityEngine;

public class AIHealth : Health
{
    private UIHealthBar healthBar;
    private float blinkDuration;
    private float timeDestroyAI;
    private AIAgent aiAgent;
    private Ragdoll ragdoll;
    private SkinnedMeshRenderer[] skinnedMeshRenderers;

    protected override void OnStart()
    {
        if (DataManager.HasInstance)
        {
            maxHealth = DataManager.Instance.globalConfig.aiMaxHealth;
            blinkDuration = DataManager.Instance.globalConfig.blinkDuration;
            timeDestroyAI = DataManager.Instance.globalConfig.timeDestroyAI;
        }

        currentHealth = maxHealth;

        aiAgent = GetComponent<AIAgent>();
        healthBar = GetComponentInChildren<UIHealthBar>();
        ragdoll = GetComponent<Ragdoll>();
        skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    protected override void OnDamage(Vector3 direction, Rigidbody rigidBody)
    {
        if (healthBar)
        {
            healthBar.SetHealthBarPercentage(currentHealth / maxHealth);
        }
        StartCoroutine(EnemyFlash());              

        Debug.Log("ai health: " + currentHealth);
    }

    protected override void OnDeath(Vector3 direction, Rigidbody ridigBody)
    {
        AIDeathState deathState = aiAgent.stateMachine.GetState(AIStateID.Death) as AIDeathState;
        deathState.direction = direction;
        deathState.rigidbody = ridigBody;
        aiAgent.stateMachine.ChangeState(AIStateID.Death);
    }

    protected override void OnHeal(float amout)
    {
        if (healthBar)
        {
            healthBar.SetHealthBarPercentage(currentHealth / maxHealth);
        }
        StartCoroutine(EnemyFlash());
    }

    public void DestroyWhenDeath()
    {
        Destroy(this.gameObject, timeDestroyAI);
    }

    private IEnumerator EnemyFlash()
    {
        foreach(SkinnedMeshRenderer skin in skinnedMeshRenderers)
        {
            skin.material.EnableKeyword("_EMISSION");
            yield return new WaitForSeconds(blinkDuration);
            skin.material.DisableKeyword("_EMISSION");
            StopCoroutine(nameof(EnemyFlash));
        }
    }
}