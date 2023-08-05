using System.Collections;
using UnityEngine;

public class AIHealth : Health
{
    private UIHealthBar healthBar;
    private float blinkDuration;
    private float timeDestroyAI;
    private AIAgent aiAgent;
    private Ragdoll ragdoll;
    private SkinnedMeshRenderer skinnedMeshRenderer;

    protected override void OnStart()
    {
        if (DataManager.HasInstance)
        {
            maxHealth = DataManager.Instance.globalConfig.maxHealth;
            blinkDuration = DataManager.Instance.globalConfig.blinkDuration;
            timeDestroyAI = DataManager.Instance.globalConfig.timeDestroyAI;
        }

        currentHealth = maxHealth;

        aiAgent = GetComponent<AIAgent>();
        healthBar = GetComponentInChildren<UIHealthBar>();
        ragdoll = GetComponent<Ragdoll>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    protected override void OnDamage(Vector3 direction, Rigidbody rigidBody)
    {
        if (healthBar != null)
        {
            healthBar.SetHealthBarPercentage(currentHealth / maxHealth);
        }

        StartCoroutine(EnemyFlash());
    }

    protected override void OnDeath(Vector3 direction, Rigidbody ridigBody)
    {
        AIDeathState deathState = aiAgent.stateMachine.GetState(AIStateID.Death) as AIDeathState;
        deathState.direction = direction;
        deathState.rigidbody = ridigBody;
        aiAgent.stateMachine.ChangeState(AIStateID.Death);
    }

    public void DestroyWhenDeath()
    {
        Destroy(this.gameObject, timeDestroyAI);
    }

    private IEnumerator EnemyFlash()
    {
        skinnedMeshRenderer.material.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(blinkDuration);
        skinnedMeshRenderer.material.DisableKeyword("_EMISSION");
        StopCoroutine(nameof(EnemyFlash));
    }
}