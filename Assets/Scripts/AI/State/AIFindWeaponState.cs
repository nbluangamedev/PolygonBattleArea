using UnityEngine;

public class AIFindWeaponState : AIState
{
    public Waypoint currentWaypoint;
    public float direction;
    private float maxHealth;

    private GameObject pickup;
    private GameObject[] pickups = new GameObject[3];

    public AIStateID GetID()
    {
        return AIStateID.FindWeapon;
    }

    public void Enter(AIAgent agent)
    {
        Debug.Log("Find weapon");
        pickup = null;

        if (DataManager.HasInstance)
        {
            agent.navMeshAgent.speed = DataManager.Instance.globalConfig.findWeaponSpeed;
        }
        agent.navMeshAgent.isStopped = false;

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

        //Pickup
        if (!pickup)
        {
            pickup = FindPickup(agent);

            if (pickup)            
            {
                CollectPickup(agent, pickup);
            }
        }

        if (agent.weapon.CountWeapon() == 1)
        {
            agent.stateMachine.ChangeState(AIStateID.FindTarget);
        }
    }

    public void Exit(AIAgent agent)
    {
        agent.navMeshAgent.isStopped = false;
    }

    private GameObject FindPickup(AIAgent agent)
    {
        Debug.Log("Find Pickup");
        int count = agent.sensor.Filter(pickups, "Pickup", "Weapon");
        Debug.Log("Count weapon: " + count);
        if (count > 0)
        {
            float bestAngle = float.MaxValue;
            GameObject bestPickup = pickups[0];
            for (int i = 0; i < count; ++i)
            {
                GameObject pickup = pickups[i];
                Debug.Log("pickups" + i + " " + pickups[i].name);
                float pickupAngle = Vector3.Angle(agent.transform.forward, pickup.transform.position - agent.transform.position);
                if (pickupAngle < bestAngle)
                {
                    bestAngle = pickupAngle;
                    bestPickup = pickup;
                }
            }
            return bestPickup;
        }
        else if (count <= 0)
        {
            WaypointPatrol(agent);
        }
        return null;
    }

    private void CollectPickup(AIAgent agent, GameObject pickup)
    {
        agent.navMeshAgent.destination = pickup.transform.position;
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