using UnityEngine;

public class AIFindTargetState : AIState
{
    public AIStateID GetID()
    {
        return AIStateID.FindTarget;
    }

    public void Enter(AIAgent agent)
    {
        agent.navMeshAgent.speed = 5f;
    }

    public void Update(AIAgent agent)
    {
        //Wander
        if (!agent.navMeshAgent.hasPath)
        {
            WorldBounds worldBounds = GameObject.FindObjectOfType<WorldBounds>();
            agent.navMeshAgent.destination = worldBounds.RandomPosition();
        }

        if (agent.targeting.HasTarget)
        {
            agent.stateMachine.ChangeState(AIStateID.Attack);
        }
    }

    public void Exit(AIAgent agent)
    {
        
    }
}
