public class AIIdleState : AIState
{
    public AIStateID GetID()
    {
        return AIStateID.Idle;
    }

    public void Enter(AIAgent agent)
    {
        agent.weapon.DeActivateWeapon();
        agent.navMeshAgent.ResetPath();
        agent.navMeshAgent.isStopped = true;
    }

    public void Update(AIAgent agent)
    {
        agent.UpdateIsDeath();

        if (agent.targeting.HasTarget)
        {
            agent.stateMachine.ChangeState(AIStateID.ChasePlayer);
        }

        if (agent.aiHealth.IsLowHealth())
        {
            if (agent.targeting.HasTarget)
            {
                agent.FaceTarget();
                agent.stateMachine.ChangeState(AIStateID.ChasePlayer);
            }
            else
            {
                agent.stateMachine.ChangeState(AIStateID.FindHealth);
            }
        }
    }

    public void Exit(AIAgent agent)
    {
        agent.navMeshAgent.isStopped = false;
    }
}