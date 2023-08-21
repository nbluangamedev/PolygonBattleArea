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
        if (DataManager.HasInstance)
        {
            agent.navMeshAgent.speed = DataManager.Instance.globalConfig.findTargetSpeed;
        }
        pickup = null;
    }

    public void Update(AIAgent agent)
    {
        agent.UpdateIsDeath();

        if (!pickup)
        {
            pickup = agent.FindPickup(pickups, "Pickup", "Health");

            if (pickup)
            {
                agent.CollectPickup(pickup);
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
}