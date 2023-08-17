using UnityEngine;

public class AIPatrolState : AIState
{
    bool walkPointSet;
    Vector3 tempTarget;

    float timer;
    float maxTime;
    float aiMaxHealth;

    public AIStateID GetID()
    {
        return AIStateID.Patrol;
    }

    public void Enter(AIAgent agent)
    {
        //randomPointOnNavMesh = agent.RandomPointOnNavMesh();
        aiMaxHealth = DataManager.Instance.globalConfig.aiMaxHealth;

        agent.navMeshAgent.isStopped = false;

        agent.navMeshAgent.speed = DataManager.Instance.globalConfig.patrolSpeed;
        agent.navMeshAgent.acceleration = DataManager.Instance.globalConfig.patrolAcceleration;
        agent.navMeshAgent.angularSpeed = DataManager.Instance.globalConfig.patrolTurnSpeed;
        maxTime = DataManager.Instance.globalConfig.patrolWaitTime;
    }

    public void Exit(AIAgent agent)
    {
        agent.navMeshAgent.isStopped = false;
    }

    public void Update(AIAgent agent)
    {
        timer -= Time.deltaTime;

        if (!agent.aiHealth.IsDead())
        {
            if (agent.FindThePlayerWithTargetingSystem())
            {
                agent.stateMachine.ChangeState(AIStateID.ChasePlayer);
            }
            else
            {
                Patrol(agent);
            }
        }
        else
        {
            agent.stateMachine.ChangeState(AIStateID.Death);
        }

        if (agent.weapon.HasWeapon())
        {
            agent.stateMachine.ChangeState(AIStateID.ChasePlayer);
        }
        else agent.stateMachine.ChangeState(AIStateID.FindWeapon);

        if (agent.aiHealth.CurrentHealth < aiMaxHealth)
        {
            agent.FaceTarget();
            if (agent.targeting.HasTarget)
            {
                agent.stateMachine.ChangeState(AIStateID.ChasePlayer);
            }
            else
            {
                agent.stateMachine.ChangeState(AIStateID.FindHealth);
            }
        }
    }

    private void SearchingPoint(AIAgent agent)
    {
        if (agent.randomPointOnNavMesh.RandomPoint(agent.transform.position, DataManager.Instance.globalConfig.patrolRadius, out Vector3 result))
        {
            tempTarget = result;
            walkPointSet = true;
        }
        else
        {
            walkPointSet = false;
        }
    }

    private void FacePatrol(AIAgent agent)
    {
        Vector3 direction = (tempTarget - agent.navMeshAgent.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        agent.navMeshAgent.transform.rotation = Quaternion.Lerp(agent.navMeshAgent.transform.rotation, lookRotation, Time.time * DataManager.Instance.globalConfig.patrolSpeed);
    }

    private void Patrol(AIAgent agent)
    {
        if (!walkPointSet)
        {
            SearchingPoint(agent);
        }

        if (walkPointSet && timer < 0f)
        {
            FacePatrol(agent);
            agent.navMeshAgent.SetDestination(tempTarget);
            //lastTempTarget = tempTarget;
            timer = maxTime;
        }

        if (agent.navMeshAgent.remainingDistance <= 0.1f)
        {
            walkPointSet = false;
        }
    }
}