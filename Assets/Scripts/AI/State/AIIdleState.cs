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
    }

    public void Update(AIAgent agent)
    {
        playerDirection = agent.transform.position - agent.playerTransform.position;
        if (playerDirection.magnitude >= maxSightDistance)
        {
            return;
        }
        else
        {
            Vector3 agentDirection = agent.transform.forward;
            playerDirection.Normalize();

            float dotProduct = Vector3.Dot(playerDirection, agentDirection);
            if (dotProduct >= 0.0f)
            {
                agent.stateMachine.ChangeState(AIStateID.ChasePlayer);
            }
        }
    }

    public void Exit(AIAgent agent)
    {

    }
}