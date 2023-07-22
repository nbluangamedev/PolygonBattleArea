using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    public AIStateMachine stateMachine;
    public AIStateID initState;
    public NavMeshAgent navMeshAgent;
    public Ragdoll ragdoll;
    public UIHealthBar healthBar;
    public Health health;
    public Transform playerTransform;
    public AIWeapon weapon;

    private void Start()
    {
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        weapon = GetComponent<AIWeapon>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        ragdoll = GetComponent<Ragdoll>();
        health = GetComponent<Health>();
        healthBar = GetComponentInChildren<UIHealthBar>();
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