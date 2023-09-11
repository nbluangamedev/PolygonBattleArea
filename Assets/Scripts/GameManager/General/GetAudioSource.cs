using UnityEngine;

public class GetAudioSource : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.Register(ListenType.GET_AUDIOSOURCE, UpdateAudioSource);
        }
    }

    private void OnDestroy()
    {
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.Unregister(ListenType.GET_AUDIOSOURCE, UpdateAudioSource);
        }
    }

    private void UpdateAudioSource(object audioClip)
    {
        if (audioClip is AudioClip clip)
        {
            audioSource.volume = PlayerPrefs.GetFloat("SE_VOLUME_KEY", 0.3f);
            audioSource.PlayOneShot(clip);
        }
    }
}