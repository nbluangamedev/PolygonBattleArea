public class AIAttackState : AIState
{
    private float closeRange;
    private float stoppingDistance;
    private float attackSpeed;

    public AIStateID GetID()
    {
        return AIStateID.Attack;
    }

    public void Enter(AIAgent agent)
    {
        if (DataManager.HasInstance)
        {
            closeRange = DataManager.Instance.globalConfig.closeRange;
            stoppingDistance = DataManager.Instance.globalConfig.stoppingDistance;
            attackSpeed = DataManager.Instance.globalConfig.attackSpeed;
        }

        agent.weapon.ActivateWeapon();
        
        agent.navMeshAgent.stoppingDistance = stoppingDistance;
        agent.navMeshAgent.speed = attackSpeed;
    }

    public void Update(AIAgent agent)
    {
        if (!agent.targeting.HasTarget)
        {
            agent.stateMachine.ChangeState(AIStateID.FindTarget);
            return;
        }

        agent.weapon.SetTarget(agent.targeting.Target.transform);
        agent.navMeshAgent.destination = agent.targeting.TargetPosition;
        SelectWeapon(agent);
        UpdateFiring(agent);
        ReloadWeapon(agent);
    }

    private void UpdateFiring(AIAgent agent)
    {
        if (agent.targeting.TargetInSight)
        {
            agent.weapon.SetFiring(true);
        }
        else agent.weapon.SetFiring(false);
    }

    public void Exit(AIAgent agent)
    {
        agent.weapon.DeActivateWeapon();
        agent.navMeshAgent.stoppingDistance = 0.0f;
    }

    private void ReloadWeapon(AIAgent agent)
    {
        var weapon = agent.weapon.currentWeapon;
        if (weapon && weapon.ammoCount <= 0)
        {
            agent.weapon.ReloadWeapon();
        }
    }

    private void SelectWeapon(AIAgent agent)
    {
        var bestWeapon = ChooseWeapon(agent);
        if (bestWeapon != agent.weapon.currentWeaponSlot)
        {
            agent.weapon.SwitchWeapon(bestWeapon);
        }
    }

    private WeaponSlot ChooseWeapon(AIAgent agent)
    {
        var distance = agent.targeting.TargetDistance;
        if (distance > closeRange)
        {
            return WeaponSlot.Primary;
        }
        else return WeaponSlot.Secondary;
    }
}