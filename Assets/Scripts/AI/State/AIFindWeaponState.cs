
using UnityEngine;

public class AIFindWeaponState : AIState
{
    public AIStateID GetID()
    {
        return AIStateID.FindWeapon;
    }

    public void Enter(AIAgent agent)
    {
        WeaponPickup pickup = FindClosetWeapon(agent);
        agent.navMeshAgent.destination = pickup.transform.position;
        agent.navMeshAgent.speed = DataManager.Instance.globalConfig.findWeaponSpeed;
    }

    public void Update(AIAgent agent)
    {
        if (agent.weapon.HasWeapon())
        {
            agent.stateMachine.ChangeState(AIStateID.Attack);
        }

        if (agent.weapon.CountWeapon() == 2)
        {
            agent.stateMachine.ChangeState(AIStateID.Attack);
        }
        else
        {
            WeaponPickup pickup = FindClosetWeapon(agent);
            if (pickup)
            {
                agent.navMeshAgent.destination = pickup.transform.position;
            }
        }
    }

    public void Exit(AIAgent agent)
    {

    }

    private WeaponPickup FindClosetWeapon(AIAgent agent)
    {
        WeaponPickup[] weapons = Object.FindObjectsOfType<WeaponPickup>();
        WeaponPickup closetWeapon = null;
        float closetDistance = float.MaxValue;
        foreach (var weapon in weapons)
        {
            float distanceToWeapon = Vector3.Distance(agent.transform.position, weapon.transform.position);
            if (distanceToWeapon < closetDistance)
            {
                closetDistance = distanceToWeapon;
                closetWeapon = weapon;
            }
        }
        return closetWeapon;
    }
}