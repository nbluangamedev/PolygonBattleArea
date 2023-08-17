using UnityEngine;

public class AIWaypointBasedPatrolState : AIState
{
    public Waypoint currentWaypoint;
    public float direction;
    float maxHealth;

    public AIStateID GetID()
    {
        return AIStateID.WaypointPatrol;
    }

    public void Enter(AIAgent agent)
    {
        if (DataManager.HasInstance)
        {
            maxHealth = DataManager.Instance.globalConfig.maxHealth;
        }

        currentWaypoint = agent.waypoints.GetComponentInChildren<Waypoint>();
        direction = Mathf.RoundToInt(Random.Range(0f, 1f));
        agent.navMeshAgent.SetDestination(currentWaypoint.GetPosition());

    }

    public void Update(AIAgent agent)
    {
        if (agent.aiHealth.IsDead())
        {
            agent.stateMachine.ChangeState(AIStateID.Death);
        }

        if (agent.weapon.HasWeapon())
        {
            agent.stateMachine.ChangeState(AIStateID.FindTarget);
            //if (agent.targeting.HasTarget)
            //{
            //    agent.FaceTarget();
            //    agent.stateMachine.ChangeState(AIStateID.Attack);
            //}
            //else WaypointPatrol(agent);
        }
        else
        {
            agent.playerSeen = false;
            agent.stateMachine.ChangeState(AIStateID.FindWeapon);
        }

        WaypointPatrol(agent);
    }

    public void Exit(AIAgent agent)
    {

    }

    private void WaypointPatrol(AIAgent agent)
    {
        if (agent.navMeshAgent.remainingDistance <= agent.navMeshAgent.stoppingDistance + 0.1f)
        {
            bool shouldBranch = false;

            if (currentWaypoint.branches != null && currentWaypoint.branches.Count > 0)
            {
                shouldBranch = Random.Range(0f, 1f) <= currentWaypoint.branchProbability;
            }

            if (shouldBranch)
            {
                currentWaypoint = currentWaypoint.branches[Random.Range(0, currentWaypoint.branches.Count - 1)];
            }
            else
            {
                if (direction == 0)
                {
                    if (currentWaypoint.nextWaypoint != null)
                    {
                        currentWaypoint = currentWaypoint.nextWaypoint;
                    }
                    else
                    {
                        currentWaypoint = currentWaypoint.prevWaypoint;
                        direction = 1;
                    }
                }

                if (direction == 1)
                {
                    if (currentWaypoint.prevWaypoint != null)
                    {
                        currentWaypoint = currentWaypoint.prevWaypoint;
                    }
                    else
                    {
                        currentWaypoint = currentWaypoint.nextWaypoint;
                        direction = 0;
                    }
                }

                agent.navMeshAgent.SetDestination(currentWaypoint.GetPosition());
            }
        }
    }
}