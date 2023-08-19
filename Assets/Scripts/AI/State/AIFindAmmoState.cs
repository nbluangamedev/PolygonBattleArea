using UnityEngine;

public class AIFindAmmoState : AIState
{
    private GameObject pickup;
    private GameObject[] pickups = new GameObject[3];
    private float timer;

    public AIStateID GetID()
    {
        return AIStateID.FindAmmo;
    }

    public void Enter(AIAgent agent)
    {
        if (DataManager.HasInstance)
        {
            agent.navMeshAgent.speed = DataManager.Instance.globalConfig.findWeaponSpeed;
        }        
        pickup = null;
        timer = 0;
    }

    public void Update(AIAgent agent)
    {
        agent.UpdateIsDeath();

        if (!pickup)
        {
            timer += Time.deltaTime;
            Debug.Log("timer find ammo " + timer);
            if (timer < 10f)
            {
                pickup = agent.FindPickup(pickups, "Pickup", "Ammo");
                if (pickup)
                {
                    agent.CollectPickup(pickup);
                }
            }
            else
            {
                timer = 0;
                agent.stateMachine.ChangeState(AIStateID.FindWeapon);
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
}