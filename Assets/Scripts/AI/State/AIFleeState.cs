using UnityEngine;

public class AIFleeState : AIState
{
    private int multiplier = 1;
    private float fleeRange;

    public AIStateID GetID()
    {
        return AIStateID.Flee;
    }

    public void Enter(AIAgent agent)
    {
        Debug.Log("Flee");
        if (DataManager.HasInstance)
        {
            fleeRange = DataManager.Instance.globalConfig.fleeRange;
        }

        agent.navMeshAgent.isStopped = false;
    }

    public void Exit(AIAgent agent)
    {
        agent.navMeshAgent.isStopped = false;
    }

    public void Update(AIAgent agent)
    {
        Vector3 runTo = agent.transform.position + (agent.transform.position - agent.playerTransform.position) * multiplier;
        float distance = Vector3.Distance(agent.transform.position, agent.playerTransform.position);
        if (distance < fleeRange)
        {
            agent.navMeshAgent.SetDestination(runTo);
        }
    }
}
