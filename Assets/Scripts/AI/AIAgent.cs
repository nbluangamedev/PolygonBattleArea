using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    public AIStateMachine stateMachine;
    public AIStateID initState;
    public NavMeshAgent navMeshAgent;
    public Ragdoll ragdoll;
    public AIHealth aiHealth;
    public UIHealthBar healthBar;
    public Transform playerTransform;

    [HideInInspector] public AIWeapon weapon;
    
    private void Start()
    {
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        navMeshAgent = GetComponent<NavMeshAgent>();
        ragdoll = GetComponent<Ragdoll>();
        aiHealth = GetComponent<AIHealth>();
        healthBar = GetComponentInChildren<UIHealthBar>();
        weapon = GetComponent<AIWeapon>();

        stateMachine = new AIStateMachine(this);
        stateMachine.RegisterState(new AIChasePlayerState());
        stateMachine.RegisterState(new AIDeathState());
        stateMachine.RegisterState(new AIIdleState());
        stateMachine.RegisterState(new AIFindWeaponState());
        stateMachine.RegisterState(new AIAttackState());
        stateMachine.ChangeState(initState);
    }

    private void Update()
    {
        stateMachine.Update();
    }
}