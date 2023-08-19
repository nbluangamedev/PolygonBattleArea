using Unity.VisualScripting;
using UnityEngine;

public class AIFindWeaponState : AIState
{
    private GameObject pickup;
    private GameObject[] pickups = new GameObject[3];

    private float timer;

    public AIStateID GetID()
    {
        return AIStateID.FindWeapon;
    }

    public void Enter(AIAgent agent)
    {
        if (DataManager.HasInstance)
        {
            agent.navMeshAgent.speed = DataManager.Instance.globalConfig.findWeaponSpeed;
        }
        agent.navMeshAgent.SetDestination(agent.currentWaypoint.GetPosition());
        agent.navMeshAgent.isStopped = false;
        pickup = null;
        timer = 0;
    }

    public void Update(AIAgent agent)
    {
        agent.UpdateIsDeath();

        if (!pickup)
        {
            timer += Time.deltaTime;
            if (timer < 30f)
            {
                pickup = agent.FindPickup(pickups, "Pickup", "Weapon");
                if (pickup)
                {
                    agent.CollectPickup(pickup);
                }
            }
            else
            {
                timer = 0;
                agent.stateMachine.ChangeState(AIStateID.FindAmmo);
            }
        }

        if (agent.weapon.CountWeapon() > 0 && !agent.weapon.IsLowAmmo())
        {
            agent.stateMachine.ChangeState(AIStateID.FindTarget);
        }
    }

    public void Exit(AIAgent agent)
    {
        agent.navMeshAgent.isStopped = false;
    }
}