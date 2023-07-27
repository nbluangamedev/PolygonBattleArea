using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIIdleState : AIState
{
    private Vector3 playerDirection;
    private float maxSightDistance;

    public AIStateID GetID()
    {
        return AIStateID.Idle;
    }

    public void Enter(AIAgent agent)
    {
        if (DataManager.HasInstance)
        {
            maxSightDistance = DataManager.Instance.globalConfig.maxDistance;
        }

        agent.weapon.DeActivateWeapon();
        agent.navMeshAgent.ResetPath();
    }

    public void Update(AIAgent agent)
    {
        if (agent.playerTransform.GetComponent<Health>().IsDead())
        {
            return;
        }

        playerDirection = agent.transform.position - agent.playerTransform.position;
        if (playerDirection.magnitude >= maxSightDistance)
        {
            return;
        }

        Vector3 agentDirection = agent.transform.forward;
        playerDirection.Normalize();

        float dotProduct = Vector3.Dot(playerDirection, agentDirection);
        if (dotProduct >= 0.0f)
        {
            agent.stateMachine.ChangeState(AIStateID.ChasePlayer);
        }
    }

    public void Exit(AIAgent agent)
    {

    }
}