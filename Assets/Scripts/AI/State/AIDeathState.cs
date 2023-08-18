using UnityEngine;

public class AIDeathState : AIState
{
    public Vector3 direction;
    public Rigidbody rigidbody;

    private float dieForce;

    public AIStateID GetID()
    {
        return AIStateID.Death;
    }

    public void Enter(AIAgent agent)
    {
        if (DataManager.HasInstance)
        {
            dieForce = DataManager.Instance.globalConfig.enemyDieForce;
        }

        agent.weapon.SetTarget(null);
        agent.navMeshAgent.enabled = false;
        agent.ragdoll.ActiveRagdoll();
        agent.ragdoll.ApplyForce(direction * dieForce, rigidbody);
        agent.weapon.DropWeapon((int)agent.weapon.currentWeaponSlot);
        agent.healthBar.Deactive();
        agent.aiHealth.DestroyWhenDeath();
    }

    public void Update(AIAgent agent)
    {

    }

    public void Exit(AIAgent agent)
    {

    }
}