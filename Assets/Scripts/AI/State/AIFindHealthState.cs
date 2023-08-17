using UnityEngine;

public class AIFindHealthState : AIState
{
    private GameObject pickup;
    private GameObject[] pickups = new GameObject[3];

    public AIStateID GetID()
    {
        return AIStateID.FindHealth;
    }

    public void Enter(AIAgent agent)
    {
        Debug.Log("Find health");
        if (DataManager.HasInstance)
        {
            agent.navMeshAgent.speed = DataManager.Instance.globalConfig.findWeaponSpeed;
        }
        pickup = null;
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
            pickup = agent.FindPickup(pickups, "Pickup", "Health");
            //pickup = FindPickup(agent);

            if (pickup)
            {
                agent.CollectPickup(pickup);
                //CollectPickup(agent, pickup);
            }
        }

        if (!agent.weapon.IsLowAmmo())
        {
            agent.stateMachine.ChangeState(AIStateID.FindTarget);
        }
    }

    public void Exit(AIAgent agent)
    {

    }

    //private GameObject FindPickup(AIAgent agent)
    //{
    //    int count = agent.sensor.Filter(pickups, "Pickup", "Health");
    //    if (count > 0)
    //    {
    //        float bestAngle = float.MaxValue;
    //        GameObject bestPickup = pickups[0];
    //        for (int i = 0; i < count; ++i)
    //        {
    //            GameObject pickup = pickups[i];
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
    //        //Debug.Log("Patrol find pickup health");
    //        agent.PatrolBasedWaypoint();
    //    }
    //    return null;
    //}

    //private void CollectPickup(AIAgent agent, GameObject pickup)
    //{
    //    agent.navMeshAgent.destination = pickup.transform.position;
    //}
}