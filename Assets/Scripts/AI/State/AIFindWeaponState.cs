using UnityEngine;

public class AIFindWeaponState : AIState
{
    private GameObject pickup;
    private GameObject[] pickups = new GameObject[3];
    WorldBounds worldBounds;

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
        worldBounds = GameObject.FindObjectOfType<WorldBounds>();
    }

    public void Update(AIAgent agent)
    {
        //Pickup
        if (!pickup)
        {
            pickup = FindPickup(agent);

            if (pickup)            
            {
                CollectPickup(agent, pickup);
            }
        }

        if (agent.weapon.CountWeapon() >= 1)
        {
            agent.stateMachine.ChangeState(AIStateID.FindTarget);
        }
    }

    public void Exit(AIAgent agent)
    {

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
            Debug.Log("Wander find pickup weapon");
            if (!agent.navMeshAgent.hasPath && !agent.navMeshAgent.pathPending)
            {                
                agent.navMeshAgent.destination = worldBounds.RandomPosition();
                Debug.Log(agent.navMeshAgent.destination);
            }
        }
        return null;
    }

    private void CollectPickup(AIAgent agent, GameObject pickup)
    {
        agent.navMeshAgent.destination = pickup.transform.position;
    }
}