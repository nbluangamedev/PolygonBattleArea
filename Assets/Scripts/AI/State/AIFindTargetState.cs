using UnityEngine;

public class AIFindTargetState : AIState
{
    public AIStateID GetID()
    {
        return AIStateID.FindTarget;
    }

    public void Enter(AIAgent agent)
    {
        Debug.Log("Find target");
        if (DataManager.HasInstance)
        {
            agent.navMeshAgent.speed = DataManager.Instance.globalConfig.findTargetSpeed;
        }
    }

    public void Update(AIAgent agent)
    {
        //Wander
        if (!agent.navMeshAgent.hasPath)
        {
            WorldBounds worldBounds = GameObject.FindObjectOfType<WorldBounds>();
            agent.navMeshAgent.destination = worldBounds.RandomPosition();
            return;
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