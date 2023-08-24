using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    public AIStateID initState;
    [HideInInspector] public AIStateMachine stateMachine;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public Ragdoll ragdoll;
    [HideInInspector] public AIHealth aiHealth;
    [HideInInspector] public UIHealthBar healthBar;
    [HideInInspector] public AIWeapon weapon;
    [HideInInspector] public AISensor sensor;
    [HideInInspector] public AITargetingSystem targeting;
    [HideInInspector] public Waypoint currentWaypoint;
    [SerializeField] private AIStateID currentState;

    public GameObject waypoints;

    private float activeWaypoint;

    private void Start()
    {
        if (!waypoints)
        {
            waypoints = GameObject.FindGameObjectWithTag("Waypoint");
        }

        currentWaypoint = waypoints.GetComponentInChildren<Waypoint>();

        healthBar = GetComponentInChildren<UIHealthBar>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        ragdoll = GetComponent<Ragdoll>();
        aiHealth = GetComponent<AIHealth>();
        weapon = GetComponent<AIWeapon>();
        sensor = GetComponentInChildren<AISensor>();
        targeting = GetComponent<AITargetingSystem>();

        stateMachine = new AIStateMachine(this);
        stateMachine.RegisterState(new AIChasePlayerState());
        stateMachine.RegisterState(new AIDeathState());
        stateMachine.RegisterState(new AIIdleState());
        stateMachine.RegisterState(new AIFindWeaponState());
        stateMachine.RegisterState(new AIFindTargetState());
        stateMachine.RegisterState(new AIAttackState());
        stateMachine.RegisterState(new AIFindHealthState());
        stateMachine.RegisterState(new AIFindAmmoState());
        stateMachine.RegisterState(new AIWaypointBasedPatrolState());
        stateMachine.ChangeState(initState);
    }

    private void Update()
    {
        activeWaypoint = Mathf.RoundToInt(Random.Range(0f, 1f));
        stateMachine.Update();
        currentState = stateMachine.currentState;
    }

    public void FaceTarget()
    {
        Vector3 direction = (targeting.TargetPosition - navMeshAgent.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.time * 720f);
    }

    public void PatrolBasedWaypoint()
    {
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance + .1f)
        {
            if (activeWaypoint == 0)
            {
                if (currentWaypoint.nextWaypoint != null)
                {
                    currentWaypoint = currentWaypoint.nextWaypoint;
                }
                else
                {
                    currentWaypoint = currentWaypoint.prevWaypoint;
                    activeWaypoint = 1;
                }
            }

            if (activeWaypoint == 1)
            {
                if (currentWaypoint.prevWaypoint != null)
                {
                    currentWaypoint = currentWaypoint.prevWaypoint;
                }
                else
                {
                    currentWaypoint = currentWaypoint.nextWaypoint;
                    activeWaypoint = 0;
                }
            }
            navMeshAgent.SetDestination(currentWaypoint.GetPosition());
        }
    }

    public GameObject FindPickup(GameObject[] pickups, string layerName, string tagName)
    {
        int count = sensor.Filter(pickups, layerName, tagName);
        if (count > 0)
        {
            float bestAngle = float.MaxValue;
            GameObject bestPickup = pickups[0];
            for (int i = 0; i < count; ++i)
            {
                GameObject pickup = pickups[i];
                float pickupAngle = Vector3.Angle(transform.forward, pickup.transform.position - transform.position);
                if (pickupAngle < bestAngle)
                {
                    bestAngle = pickupAngle;
                    bestPickup = pickup;
                }
            }
            return bestPickup;
        }
        else if (count <= 0)
        {
            PatrolBasedWaypoint();
        }
        return null;
    }

    public void CollectPickup(GameObject pickup)
    {
        navMeshAgent.destination = pickup.transform.position;
    }

    public void UpdateLowHealth()
    {
        if (aiHealth.IsLowHealth())
        {
            stateMachine.ChangeState(AIStateID.FindHealth);
        }
    }

    public void UpdateLowAmmo()
    {
        if (weapon.IsLowAmmo())
        {
            stateMachine.ChangeState(AIStateID.FindAmmo);
        }
    }

    public void UpdateIsDeath()
    {
        if (aiHealth.IsDead())
        {
            stateMachine.ChangeState(AIStateID.Death);
        }
    }

    public bool CheckWeaponRemain(bool canSwitchWeapon)
    {
        int count = 0;
        if (canSwitchWeapon)
        {
            for (int i = 0; i < 2; i++)
            {
                if (weapon.aiWeapons[i].IsEmptyAmmo())
                    count++;
            }
        }
        else return false;

        if (count == 2)
            return false;
        return true;
    }

    public void SwitchWeapon()
    {
        int currentSlot = (int)weapon.currentWeaponSlot;
        int switchSlot = 1 - currentSlot;
        weapon.SwitchWeapon((WeaponSlot)switchSlot);
    }
}