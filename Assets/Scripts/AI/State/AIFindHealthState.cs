using UnityEngine;

public class AIFindHealthState : AIState
{
    public AIStateID GetID()
    {
        return AIStateID.FindHealth;
    }

    public void Enter(AIAgent agent)
    {
        Debug.Log("Find health");
        HealthPickup health = FindClosetHealth(agent);
        if (health)
        {
            agent.navMeshAgent.destination = health.transform.position;
            agent.navMeshAgent.speed = DataManager.Instance.globalConfig.findWeaponSpeed;
        }
    }

    public void Update(AIAgent agent)
    {
        if (!agent.aiHealth.IsLowHealth())
        {
            agent.stateMachine.ChangeState(AIStateID.FindTarget);
        }

        if (agent.aiHealth.IsLowHealth())
        {
            agent.stateMachine.ChangeState(AIStateID.FindTarget);
        }

        HealthPickup health = FindClosetHealth(agent);
        if (health)
        {
            agent.navMeshAgent.destination = health.transform.position;
            agent.navMeshAgent.speed = DataManager.Instance.globalConfig.findWeaponSpeed;
        }
    }

    public void Exit(AIAgent agent)
    {

    }

    private HealthPickup FindClosetHealth(AIAgent agent)
    {
        HealthPickup[] healths = Object.FindObjectsOfType<HealthPickup>();
        HealthPickup closetHealth = null;
        float closetDistance = float.MaxValue;
        foreach (HealthPickup health in healths)
        {
            float distanceToHealth = Vector3.Distance(agent.transform.position, health.transform.position);
            if (distanceToHealth < closetDistance)
            {
                closetDistance = distanceToHealth;
                closetHealth = health;
            }
        }
        return closetHealth;
    }
}