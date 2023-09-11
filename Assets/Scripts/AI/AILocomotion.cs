using UnityEngine;
using UnityEngine.AI;

public class AILocomotion : MonoBehaviour
{
    public WeaponAnimationEvent animationEvents;

    private NavMeshAgent navMeshAgent;
    private Animator animator;

    private void Awake()
    {
        animationEvents = GetComponent<WeaponAnimationEvent>();
    }

    void Start()
    {
        animationEvents.weaponAnimationEvent.AddListener(OnAnimationMoveEvent);
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (navMeshAgent.hasPath)
        {
            animator.SetFloat("speed", navMeshAgent.velocity.magnitude);
        }
        else
        {
            animator.SetFloat("speed", 0);
        }
    }

    private void OnAnimationMoveEvent(string eventName)
    {
        switch (eventName)
        {
            case "Sprinting":
                SprintSound();
                break;
            case "Locomotion":
                LocomotionSound();
                break;
        }
    }


    private void SprintSound()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySEAgent(AUDIO.SE_MOVEMENT, AudioManager.Instance.AttachSESource.volume);
        }
    }

    private void LocomotionSound()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySEAgent(AUDIO.SE_MOVEMENT, AudioManager.Instance.AttachSESource.volume);
        }
    }
}