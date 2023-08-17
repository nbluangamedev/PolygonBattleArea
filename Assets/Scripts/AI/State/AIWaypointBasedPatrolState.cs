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
        if (agent.aiHealth.IsDead())
        {
            agent.stateMachine.ChangeState(AIStateID.Death);
        }

        if (agent.aiHealth.IsLowHealth())
        {
            agent.stateMachine.ChangeState(AIStateID.FindHealth);
        }

        if (agent.weapon.IsLowAmmo())
        {
            agent.stateMachine.ChangeState(AIStateID.FindAmmo);
        }

        if (agent.weapon.HasWeapon())
        {
            agent.stateMachine.ChangeState(AIStateID.FindTarget);
        }
        else
        {
            agent.playerSeen = false;
            agent.stateMachine.ChangeState(AIStateID.FindWeapon);
        }

        if (agent.targeting.HasTarget)
        {
            if (agent.weapon.HasWeapon())
            {
                agent.stateMachine.ChangeState(AIStateID.Attack);
            }
            else agent.stateMachine.ChangeState(AIStateID.FindWeapon);
        }

        agent.PatrolBasedWaypoint();
    }

    public void Exit(AIAgent agent)
    {

    }
}