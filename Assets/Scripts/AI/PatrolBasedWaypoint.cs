using UnityEngine;

public class PatrolBasedWaypoint : MonoBehaviour
{
    public void WaypointPatrol(AIAgent agent, Waypoint currentWaypoint, float activeWaypoint)
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
                if (activeWaypoint == 0)
                {
                    if (currentWaypoint.nextWaypoint != null)
                    {
                        currentWaypoint = currentWaypoint.nextWaypoint;
                    }
                    else
                    {
                        currentWaypoint = currentWaypoint.prevWaypoint;
                        activeWaypoint = 1;
                    }
                }

                if (activeWaypoint == 1)
                {
                    if (currentWaypoint.prevWaypoint != null)
                    {
                        currentWaypoint = currentWaypoint.prevWaypoint;
                    }
                    else
                    {
                        currentWaypoint = currentWaypoint.nextWaypoint;
                        activeWaypoint = 0;
                    }
                }

                agent.navMeshAgent.SetDestination(currentWaypoint.GetPosition());
            }
        }
    }
}
