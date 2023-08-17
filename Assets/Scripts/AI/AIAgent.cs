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
    [HideInInspector] public Transform playerTransform;
    [HideInInspector] public RandomPointOnNavMesh randomPointOnNavMesh;
    [SerializeField] private AIStateID currentState;

    public GameObject waypoints;
    public bool playerSeen = false;

    private void Start()
    {
        if (!playerTransform)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        else Debug.LogError("No player object with Player tag found!");

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
        stateMachine.Update();
        currentState = stateMachine.currentState;
    }

    public bool FindThePlayerWithTargetingSystem()
    {
        //Finding Player by using Targeting System
        if (targeting.HasTarget)
        {
            if (targeting.Target.CompareTag("Player"))
            {
                playerSeen = true;
                return true;
            }
        }
        playerSeen = false;
        return false;
    }

    public void FaceTarget()
    {
        Vector3 direction = (targeting.TargetPosition - navMeshAgent.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.time * 720f);
    }

    //public void FacePlayer()
    //{
    //    Vector3 direction = (playerTransform.position - navMeshAgent.transform.position).normalized;
    //    Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
    //    transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.time * 720f);
    //}       

    //public bool FindThePlayer()
    //{
    //    //Finding Player by using only Sensor
    //    for (int i = 0; i < sensor.Objects.Count; i++)
    //    {
    //        if (sensor.Objects[i].CompareTag("Player"))
    //        {
    //            playerSeen = true;
    //            return true;
    //        }
    //    }
    //    return false;
    //}
}