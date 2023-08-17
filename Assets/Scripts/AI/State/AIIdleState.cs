public class AIIdleState : AIState
{
    private float maxHealth;

    public AIStateID GetID()
    {
        return AIStateID.Idle;
    }

    public void Enter(AIAgent agent)
    {
        if (DataManager.HasInstance)
        {
            maxHealth = DataManager.Instance.globalConfig.maxHealth;
        }

        agent.weapon.DeActivateWeapon();
        agent.navMeshAgent.ResetPath();

        agent.navMeshAgent.isStopped = true;
    }

    public void Update(AIAgent agent)
    {
        if (agent.aiHealth.IsDead())
        {
            agent.stateMachine.ChangeState(AIStateID.Death);
        }

        if (agent.targeting.HasTarget)
        {
            agent.stateMachine.ChangeState(AIStateID.ChasePlayer);
        }

        if (agent.aiHealth.CurrentHealth < maxHealth)
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

    public void Exit(AIAgent agent)
    {
        agent.navMeshAgent.isStopped = false;
    }
}