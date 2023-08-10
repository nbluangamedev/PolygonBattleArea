using UnityEngine;

public class AIFindWeaponState : AIState
{
    GameObject pickup;
    GameObject[] pickups = new GameObject[1];

    public AIStateID GetID()
    {
        return AIStateID.FindWeapon;
    }

    public void Enter(AIAgent agent)
    {
        pickup = null;
        if (DataManager.HasInstance)
        {
            agent.navMeshAgent.speed = DataManager.Instance.globalConfig.findWeaponSpeed;
        }
    }

    public void Update(AIAgent agent)
    {
        //Find pickup
        if (pickup == null)
        {
            pickup = FindPickup(agent);
            if (pickup)
            {
                CollectPickup(agent, pickup);
            }
        }

        //Wander
        if (!agent.navMeshAgent.hasPath)
        {
            WorldBounds worldBounds = GameObject.FindObjectOfType<WorldBounds>();
            agent.navMeshAgent.destination = worldBounds.RandomPosition();
        }

        if (agent.weapon.CountWeapon() == 1)
        {
            agent.stateMachine.ChangeState(AIStateID.FindTarget);
        }
    }

    public void Exit(AIAgent agent)
    {

    }

    private GameObject FindPickup(AIAgent agent)
    {
        int count = agent.sensor.Filter(pickups, "Pickup");

        if (count > 0)
        {
            return pickups[0];
        }
        return null;
    }

    private void CollectPickup(AIAgent agent, GameObject pickup)
    {
        agent.navMeshAgent.destination = pickup.transform.position;
    }
}