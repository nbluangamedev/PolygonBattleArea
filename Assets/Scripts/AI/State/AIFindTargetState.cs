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
            agent.navMeshAgent.acceleration = DataManager.Instance.globalConfig.findTargetAcceleration;
            agent.navMeshAgent.angularSpeed = DataManager.Instance.globalConfig.findTargetTurnSpeed;
        }

        agent.navMeshAgent.SetDestination(agent.currentWaypoint.GetPosition());
        agent.navMeshAgent.isStopped = false;
    }

    public void Update(AIAgent agent)
    {
        if (agent.targeting.HasTarget)
        {
            agent.stateMachine.ChangeState(AIStateID.Attack);
        }
        else agent.PatrolBasedWaypoint();
    }

    public void Exit(AIAgent agent)
    {
        agent.navMeshAgent.isStopped = false;
    }
}