using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackState : AIState
{
    public AIStateID GetID()
    {
        return AIStateID.Attack;
    }

    public void Enter(AIAgent agent)
    {
        agent.weapon.ActivateWeapon();
        agent.weapon.SetTarget(agent.playerTransform);
        agent.navMeshAgent.stoppingDistance = 5.0f;
    }

    public void Update(AIAgent agent)
    {
        agent.navMeshAgent.destination = agent.playerTransform.position;
    }

    public void Exit(AIAgent agent)
    {
        agent.navMeshAgent.stoppingDistance = 0.0f;
    }
}