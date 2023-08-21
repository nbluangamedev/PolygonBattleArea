using UnityEngine;

public class AIWaypointBasedPatrolState : AIState
{
    public AIStateID GetID()
    {
        return AIStateID.WaypointPatrol;
    }

    public void Enter(AIAgent agent)
    {
        if (DataManager.HasInstance)
        {
            agent.navMeshAgent.speed = DataManager.Instance.globalConfig.patrolSpeed;
        }

        agent.navMeshAgent.SetDestination(agent.currentWaypoint.GetPosition());
    }

    public void Update(AIAgent agent)
    {
        agent.UpdateIsDeath();
        agent.UpdateLowAmmo();
        agent.UpdateLowHealth();

        if (agent.weapon.HasWeapon())
        {
            agent.stateMachine.ChangeState(AIStateID.FindTarget);
        }
        else
        {
            agent.stateMachine.ChangeState(AIStateID.FindWeapon);
        }

        agent.PatrolBasedWaypoint();
    }

    public void Exit(AIAgent agent)
    {

    }
}