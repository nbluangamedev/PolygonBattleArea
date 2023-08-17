using UnityEngine;
using UnityEngine.AI;

public class AIFindTargetState : AIState
{
    public Waypoint currentWaypoint;
    public float direction;

    private float maxHealth;
    private float findTargetTurnSpeed;

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
            findTargetTurnSpeed = DataManager.Instance.globalConfig.findTargetTurnSpeed;
            maxHealth = DataManager.Instance.globalConfig.maxHealth;
        }

        agent.navMeshAgent.angularSpeed = findTargetTurnSpeed;
        agent.navMeshAgent.isStopped = false;

        currentWaypoint = agent.waypoints.GetComponentInChildren<Waypoint>();
        direction = Mathf.RoundToInt(Random.Range(0f, 1f));
        agent.navMeshAgent.SetDestination(currentWaypoint.GetPosition());
    }

    public void Update(AIAgent agent)
    {
        if(!agent.targeting.HasTarget)
        {
            //Debug.Log("Dont have target, patrol");
            WaypointPatrol(agent);
        }
        else agent.stateMachine.ChangeState(AIStateID.Attack);
                
        //else
        //{
        //    if (agent.aiHealth.IsDead())
        //    {
        //        agent.stateMachine.ChangeState(AIStateID.Death);
        //    }
        //    else if (agent.aiHealth.CurrentHealth < maxHealth)
        //    {
        //        agent.stateMachine.ChangeState(AIStateID.FindHealth);
        //    }
        //    else if (agent.weapon.IsLowAmmo())
        //    {
        //        agent.stateMachine.ChangeState(AIStateID.FindAmmo);
        //    }
        //    else agent.stateMachine.ChangeState(AIStateID.WaypointPatrol);
        //}

        //if (agent.weapon.CountWeapon() < 2)
        //{
        //    agent.stateMachine.ChangeState(AIStateID.FindWeapon);
        //}
        //else if (agent.aiHealth.CurrentHealth < maxHealth)
        //{
        //    agent.stateMachine.ChangeState(AIStateID.FindHealth);
        //}
    }

    public void Exit(AIAgent agent)
    {
        agent.navMeshAgent.isStopped = false;
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
                Debug.Log(currentWaypoint.name);
                agent.navMeshAgent.SetDestination(currentWaypoint.GetPosition());
            }
        }
    }
}