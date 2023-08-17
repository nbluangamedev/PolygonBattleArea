using UnityEngine;

public class AIChasePlayerState : AIState
{
    private float attackRadius;

    public AIStateID GetID()
    {
        return AIStateID.ChasePlayer;
    }

    public void Enter(AIAgent agent)
    {
        if (DataManager.HasInstance)
        {
            attackRadius = DataManager.Instance.globalConfig.attackRadius;
        }

        agent.playerSeen = true;
        agent.navMeshAgent.stoppingDistance = attackRadius;
        agent.navMeshAgent.isStopped = false;

    }

    public void Update(AIAgent agent)
    {
        if (agent.targeting.HasTarget)
        {
            float distance = Vector3.Distance(agent.targeting.TargetPosition, agent.transform.position);
            if (distance < attackRadius)
            {
                agent.stateMachine.ChangeState(AIStateID.Attack);
            }
            else
            {
                agent.navMeshAgent.destination = agent.targeting.TargetPosition;
            }
        }
        else agent.stateMachine.ChangeState(AIStateID.WaypointPatrol);
    }

    public void Exit(AIAgent agent)
    {
        agent.navMeshAgent.stoppingDistance = 0.0f;
        agent.playerSeen = false;
        agent.navMeshAgent.isStopped = false;
    }
}