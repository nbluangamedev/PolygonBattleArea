using UnityEngine;

public class AIFindAmmoState : AIState
{
    private GameObject pickup;
    private GameObject[] pickups = new GameObject[3];

    public AIStateID GetID()
    {
        return AIStateID.FindAmmo;
    }

    public void Enter(AIAgent agent)
    {
        Debug.Log("Find ammo");
        pickup = null;

        if (DataManager.HasInstance)
        {
            agent.navMeshAgent.speed = DataManager.Instance.globalConfig.findWeaponSpeed;
        }
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

        if (!agent.aiHealth.IsLowHealth())
        {
            agent.stateMachine.ChangeState(AIStateID.FindTarget);
        }
    }

    public void Exit(AIAgent agent)
    {

    }

    private GameObject FindPickup(AIAgent agent)
    {
        int count = agent.sensor.Filter(pickups, "Pickup", "Ammo");
        if (count > 0)
        {
            float bestAngle = float.MaxValue;
            GameObject bestPickup = pickups[0];
            for (int i = 0; i < count; ++i)
            {
                GameObject pickup = pickups[i];
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
            Debug.Log("Wander find pickup ammo");
            WorldBounds worldBounds = GameObject.FindObjectOfType<WorldBounds>();
            agent.navMeshAgent.destination = worldBounds.RandomPosition();
        }
        return null;
    }

    private void CollectPickup(AIAgent agent, GameObject pickup)
    {
        agent.navMeshAgent.destination = pickup.transform.position;
    }
}