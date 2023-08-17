using UnityEngine;

public class AIFindWeaponState : AIState
{
    private GameObject pickup;
    private GameObject[] pickups = new GameObject[3];

    public AIStateID GetID()
    {
        return AIStateID.FindWeapon;
    }

    public void Enter(AIAgent agent)
    {
        Debug.Log("Find weapon");
        if (DataManager.HasInstance)
        {
            agent.navMeshAgent.speed = DataManager.Instance.globalConfig.findWeaponSpeed;
        }
        pickup = null;
        agent.navMeshAgent.SetDestination(agent.currentWaypoint.GetPosition());
        agent.navMeshAgent.isStopped = false;
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
            pickup = agent.FindPickup(pickups, "Pickup", "Weapon");
            //pickup = FindPickup(agent);

            if (pickup)            
            {
                agent.CollectPickup(pickup);
                //CollectPickup(agent, pickup);
            }
        }

        if (agent.weapon.CountWeapon() > 0)
        {
            agent.stateMachine.ChangeState(AIStateID.FindTarget);
        }
    }

    public void Exit(AIAgent agent)
    {
        agent.navMeshAgent.isStopped = false;
    }

    //private GameObject FindPickup(AIAgent agent)
    //{
    //    //Debug.Log("Find Pickup");
    //    int count = agent.sensor.Filter(pickups, "Pickup", "Weapon");
    //    //Debug.Log("Count weapon: " + count);
    //    if (count > 0)
    //    {
    //        float bestAngle = float.MaxValue;
    //        GameObject bestPickup = pickups[0];
    //        for (int i = 0; i < count; ++i)
    //        {
    //            GameObject pickup = pickups[i];
    //            Debug.Log("pickups" + i + " " + pickups[i].name);
    //            float pickupAngle = Vector3.Angle(agent.transform.forward, pickup.transform.position - agent.transform.position);
    //            if (pickupAngle < bestAngle)
    //            {
    //                bestAngle = pickupAngle;
    //                bestPickup = pickup;
    //            }
    //        }
    //        return bestPickup;
    //    }
    //    else if (count <= 0)
    //    {
    //        agent.PatrolBasedWaypoint();
    //    }
    //    return null;
    //}

    //private void CollectPickup(AIAgent agent, GameObject pickup)
    //{
    //    agent.navMeshAgent.destination = pickup.transform.position;
    //}
}