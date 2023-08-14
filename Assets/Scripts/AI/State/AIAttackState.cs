using UnityEngine;

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
        Debug.Log("Attack state");
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
        ReloadWeapon(agent);
        SelectWeapon(agent);
        UpdateFiring(agent);
        UpdateLowHealth(agent);
        UpdateLowAmmo(agent);
    }

    public void Exit(AIAgent agent)
    {
        agent.weapon.DeActivateWeapon();
        agent.navMeshAgent.stoppingDistance = 0.0f;
    }

    private void UpdateFiring(AIAgent agent)
    {
        if (agent.targeting.TargetInSight)
        {
            agent.weapon.SetFiring(true);
        }
        else
        {
            agent.weapon.SetFiring(false);
        }
    }

    private void ReloadWeapon(AIAgent agent)
    {
        var weapon = agent.weapon.AICurrentWeapon;
        if (weapon && weapon.ShouldReload())
        {
            agent.weapon.ReloadWeapon();
        }
    }

    private void UpdateLowHealth(AIAgent agent)
    {
        if (agent.aiHealth.IsLowHealth())
        {
            agent.stateMachine.ChangeState(AIStateID.FindHealth);
        }
    }

    private void UpdateLowAmmo(AIAgent agent)
    {
        if (agent.weapon.IsLowAmmo())
        {
            agent.stateMachine.ChangeState(AIStateID.FindAmmo);
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
        bool hasWeapon = agent.weapon.CountWeapon() == 2;
        if (hasWeapon)
        {
            foreach(var weaponTMP in agent.weapon.aiWeapons)
            if (!weaponTMP.IsEmptyAmmo())
            {
                return weaponTMP.weaponSlot;
            }
            return agent.weapon.AICurrentWeapon.weaponSlot;
        }
        return agent.weapon.AICurrentWeapon.weaponSlot;
    }
}