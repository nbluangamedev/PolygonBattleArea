using System.Collections;
using UnityEngine;

public class AIFindAmmoState : AIState
{
    public AIStateID GetID()
    {
        return AIStateID.FindAmmo;
    }

    public void Enter(AIAgent agent)
    {
        Debug.Log("Find ammo");
        AmmoPickup ammo = FindClosetAmmo(agent);
        if (ammo)
        {
            agent.navMeshAgent.destination = ammo.transform.position;
            agent.navMeshAgent.speed = DataManager.Instance.globalConfig.findWeaponSpeed;
        }
    }

    public void Update(AIAgent agent)
    {
        if (!agent.weapon.IsLowAmmo())
        {
            agent.stateMachine.ChangeState(AIStateID.FindTarget);
        }

        if (agent.weapon.IsLowAmmo())
        {
            agent.stateMachine.ChangeState(AIStateID.FindTarget);
        }

        AmmoPickup ammo = FindClosetAmmo(agent);
        if (ammo)
        {
            agent.navMeshAgent.destination = ammo.transform.position;
            agent.navMeshAgent.speed = DataManager.Instance.globalConfig.findWeaponSpeed;
        }
    }

    public void Exit(AIAgent agent)
    {

    }

    private AmmoPickup FindClosetAmmo(AIAgent agent)
    {
        AmmoPickup[] ammos = Object.FindObjectsOfType<AmmoPickup>();
        AmmoPickup closetAmmo = null;
        float closetDistance = float.MaxValue;
        foreach (AmmoPickup ammo in ammos)
        {
            float distanceToAmmo = Vector3.Distance(agent.transform.position, ammo.transform.position);
            if (distanceToAmmo < closetDistance)
            {
                closetDistance = distanceToAmmo;
                closetAmmo = ammo;
            }
        }
        return closetAmmo;
    }
}