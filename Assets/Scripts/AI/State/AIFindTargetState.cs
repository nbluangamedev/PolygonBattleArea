using UnityEngine;

public class AIFindTargetState : AIState
{
    WorldBounds worldBounds;
    Transform playerTransform;

    public AIStateID GetID()
    {
        return AIStateID.FindTarget;
    }

    public void Enter(AIAgent agent)
    {
        Debug.Log("Find target");
        if (DataManager.HasInstance)
        {
            agent.navMeshAgent.speed = DataManager.Instance.globalConfig.findTargetSpeed;
        }
        worldBounds = GameObject.FindObjectOfType<WorldBounds>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Update(AIAgent agent)
    {
        if (agent.navMeshAgent.hasPath)
        {
            Debug.Log("haspath");
            if (!agent.targeting.HasTarget)
            {
                if (playerTransform)
                    agent.navMeshAgent.destination = playerTransform.position;
            }
        }

        //Wander
        if (!agent.navMeshAgent.hasPath && !agent.navMeshAgent.pathPending)
        {
            Debug.Log("wander find target");
            agent.navMeshAgent.destination = worldBounds.RandomPosition();
            return;
        }


        if (agent.targeting.HasTarget)
        {
            agent.stateMachine.ChangeState(AIStateID.Attack);
        }
    }

    public void Exit(AIAgent agent)
    {

    }
}