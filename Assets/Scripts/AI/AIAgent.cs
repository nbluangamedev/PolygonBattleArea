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
    [HideInInspector] public Transform playerTransform;
    [HideInInspector] public AIWeapon weapon;
    [HideInInspector] public AISensor sensor;
    [HideInInspector] public AITargetingSystem targeting;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        ragdoll = GetComponent<Ragdoll>();
        aiHealth = GetComponent<AIHealth>();
        healthBar = GetComponentInChildren<UIHealthBar>();
        weapon = GetComponent<AIWeapon>();
        sensor = GetComponent<AISensor>();
        targeting = GetComponent<AITargetingSystem>();
        if (!playerTransform)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        stateMachine = new AIStateMachine(this);
        stateMachine.RegisterState(new AIChasePlayerState());
        stateMachine.RegisterState(new AIDeathState());
        stateMachine.RegisterState(new AIIdleState());
        stateMachine.RegisterState(new AIFindWeaponState());
        stateMachine.RegisterState(new AIAttackState());
        stateMachine.RegisterState(new AIFindTargetState());
        stateMachine.RegisterState(new AIFindHealthState());
        stateMachine.RegisterState(new AIFindAmmoState());
        stateMachine.ChangeState(initState);
    }

    private void Update()
    {
        stateMachine.Update();
    }
}