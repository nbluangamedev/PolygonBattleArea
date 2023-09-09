using UnityEngine;

public class GetAudioManager : MonoBehaviour
{
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();    
    }

    private void Start()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.AttachSESource = audioSource;
        }
    }
}
