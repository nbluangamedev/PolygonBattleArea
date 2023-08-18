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
        if (DataManager.HasInstance)
        {
            agent.navMeshAgent.stoppingDistance = DataManager.Instance.globalConfig.attackStoppingDistance;
            agent.navMeshAgent.speed = DataManager.Instance.globalConfig.attackMoveSpeed;
            attackRadius = DataManager.Instance.globalConfig.attackRadius;
        }

        //agent.FaceTarget();
        agent.weapon.ActivateWeapon();
        agent.navMeshAgent.isStopped = true;
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
            if (agent.targeting.HasTarget && agent.targeting.TargetDistance <= attackRadius)
            {
                //agent.playerSeen = true;
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
        
        //if (agent.weapon.IsLowAmmo())
        //{
        //    if (agent.weapon.CountWeapon() == 2)
        //    {
        //        agent.SelectWeapon();
        //    }
        //}

        agent.UpdateLowAmmo();
    }

    public void Exit(AIAgent agent)
    {
        agent.weapon.DeActivateWeapon();
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