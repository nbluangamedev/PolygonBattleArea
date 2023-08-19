using UnityEngine;

public class AIAttackState : AIState
{
    private float attackRadius;
    private bool canSwitchWeapon;
    private bool isWeaponRemain;

    public AIStateID GetID()
    {
        return AIStateID.Attack;
    }

    public void Enter(AIAgent agent)
    {
        if (DataManager.HasInstance)
        {
            agent.navMeshAgent.stoppingDistance = DataManager.Instance.globalConfig.attackStoppingDistance;
            agent.navMeshAgent.speed = DataManager.Instance.globalConfig.attackMoveSpeed;
            attackRadius = DataManager.Instance.globalConfig.attackRadius;
        }

        agent.weapon.ActivateWeapon();
        agent.navMeshAgent.isStopped = true;
        canSwitchWeapon = agent.weapon.CountWeapon() == 2;
        isWeaponRemain = agent.CheckWeaponRemain(canSwitchWeapon);
    }

    public void Update(AIAgent agent)
    {
        agent.UpdateIsDeath();
        agent.UpdateLowHealth();

        if (!agent.targeting.HasTarget)
        {
            agent.stateMachine.ChangeState(AIStateID.FindTarget);
        }

        if (agent.weapon.HasWeapon())
        {
            if (agent.weapon.IsLowAmmo())
            {
                isWeaponRemain = agent.CheckWeaponRemain(canSwitchWeapon);
                if (isWeaponRemain)
                {
                    agent.SwitchWeapon();
                }
                else
                {
                    agent.stateMachine.ChangeState(AIStateID.FindAmmo);
                }
            }

            if (agent.targeting.HasTarget && agent.targeting.TargetDistance <= attackRadius)
            {
                agent.FaceTarget();
                agent.weapon.SetTarget(agent.targeting.Target.transform);
                agent.navMeshAgent.destination = agent.targeting.TargetPosition;
                ReloadWeapon(agent);
                UpdateFiring(agent);
            }
            else if (agent.targeting.HasTarget && agent.targeting.TargetDistance > attackRadius)
            {
                agent.FaceTarget();
                agent.stateMachine.ChangeState(AIStateID.ChasePlayer);
            }
        }

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
}