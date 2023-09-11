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
        if (agent.ragdoll.animator.enabled == true)
        {
            if (ListenerManager.HasInstance)
            {
                ListenerManager.Instance.BroadCast(ListenType.ENEMY_COUNT, 1);
            }
        }
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySEAgent(AUDIO.SE_DIE1);
        }
        agent.weapon.DropWeaponPrefab((int)agent.weapon.currentWeaponSlot);
        agent.ragdoll.ActiveRagdoll();
        agent.ragdoll.ApplyForce(direction * dieForce, rigidbody);
        agent.healthBar.Deactive();
        agent.weapon.SetTarget(null);
        agent.navMeshAgent.enabled = false;
        agent.aiHealth.DestroyWhenDeath();
    }

    public void Update(AIAgent agent)
    {

    }

    public void Exit(AIAgent agent)
    {

    }
}