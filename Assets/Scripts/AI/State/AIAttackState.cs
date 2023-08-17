using UnityEngine;

public class AIAttackState : AIState
{
    private float attackRadius;

    public AIStateID GetID()
    {
        return AIStateID.Attack;
    }

    public void Enter(AIAgent agent)
    {
        Debug.Log("Attack state");
        if (DataManager.HasInstance)
        {
            agent.navMeshAgent.stoppingDistance = DataManager.Instance.globalConfig.attackStoppingDistance;
            agent.navMeshAgent.speed = DataManager.Instance.globalConfig.attackMoveSpeed;
            attackRadius = DataManager.Instance.globalConfig.attackRadius;
        }

        agent.FaceTarget();
        agent.weapon.ActivateWeapon();
        agent.navMeshAgent.isStopped = true;
    }

    public void Update(AIAgent agent)
    {
        if (agent.aiHealth.IsDead())
        {
            agent.stateMachine.ChangeState(AIStateID.Death);
        }

        if (!agent.targeting.HasTarget)
        {
            agent.stateMachine.ChangeState(AIStateID.FindTarget);
        }

        if (agent.targeting.HasTarget && agent.targeting.TargetDistance <= attackRadius)
        {
            agent.weapon.SetTarget(agent.targeting.Target.transform);
            agent.navMeshAgent.destination = agent.targeting.TargetPosition;
            ReloadWeapon(agent);
            SelectWeapon(agent);
            UpdateFiring(agent);
            UpdateLowHealth(agent);
            UpdateLowAmmo(agent);
        }
        else if(agent.targeting.HasTarget && agent.targeting.TargetDistance > attackRadius)
        {
            agent.stateMachine.ChangeState(AIStateID.ChasePlayer);
        }
        else agent.stateMachine.ChangeState(AIStateID.FindTarget);
    }

    public void Exit(AIAgent agent)
    {
        agent.weapon.DeActivateWeapon();
        agent.navMeshAgent.stoppingDistance = 0.0f;
        agent.navMeshAgent.isStopped = false;
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
            foreach (var weaponTMP in agent.weapon.aiWeapons)
                if (!weaponTMP.IsEmptyAmmo())
                {
                    return weaponTMP.weaponSlot;
                }
            return agent.weapon.AICurrentWeapon.weaponSlot;
        }
        return agent.weapon.AICurrentWeapon.weaponSlot;
    }
}