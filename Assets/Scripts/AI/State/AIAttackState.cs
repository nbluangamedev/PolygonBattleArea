using UnityEngine;

public class AIAttackState : AIState
{
    private float closeRange;

    public AIStateID GetID()
    {
        return AIStateID.Attack;
    }

    public void Enter(AIAgent agent)
    {
        if (DataManager.HasInstance)
        {
            closeRange = DataManager.Instance.globalConfig.closeRange;
        }

        agent.weapon.ActivateWeapon();
        agent.weapon.SetTarget(agent.playerTransform);
        agent.navMeshAgent.stoppingDistance = 15.0f;
        agent.weapon.SetFiring(true);
    }

    public void Update(AIAgent agent)
    {
        agent.navMeshAgent.destination = agent.playerTransform.position;
        ReloadWeapon(agent);
        SelectWeapon(agent);
        if (agent.playerTransform.GetComponent<Health>().IsDead())
        {
            agent.stateMachine.ChangeState(AIStateID.Idle);
        }
    }

    public void Exit(AIAgent agent)
    {
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
        if(bestWeapon!=agent.weapon.currentWeaponSlot)
        {
            agent.weapon.SwitchWeapon(bestWeapon);
        }
        agent.weapon.SetFiring(true);
    }

    private WeaponSlot ChooseWeapon(AIAgent agent)
    {
        var distance = Vector3.Distance(agent.playerTransform.position, agent.transform.position);
        if (distance > closeRange)
        {
            return WeaponSlot.Primary;
        }
        else return WeaponSlot.Secondary;
    }
}