using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : BaseManager<AudioManager>
{
    //key and default value for saving volume
    private const string BGM_VOLUME_KEY = "BGM_VOLUME_KEY";
    private const string SE_VOLUME_KEY = "SE_VOLUME_KEY";
    //private const string VOICE_VOLUME_KEY = "SE_VOLUME_KEY";        //add
    private const float BGM_VOLUME_DEFULT = 0.2f;
    private const float SE_VOLUME_DEFULT = 0.3f;
    //private const float VOICE_VOLUME_DEFULT = 0.5f;         //add

    //Time it takes for the background music to fade
    public const float BGM_FADE_SPEED_RATE_HIGH = 0.9f;
    public const float BGM_FADE_SPEED_RATE_LOW = 0.3f;
    private float bgmFadeSpeedRate = BGM_FADE_SPEED_RATE_HIGH;

    //Next BGM name, SE name
    private string nextBGMName;
    private string nextSEName;

    private int enemyKey;     //add
    private int playerKey;
    private int talkKey;

    //Is the background music fading out?
    private bool isFadeOut = false;

    //Separate audio sources for BGM and SE
    public AudioSource AttachBGMSource;
    public AudioSource AttachSESource;

    public AudioSource AttachVOICESource;       //add

    //Keep All Audio
    private Dictionary<string, AudioClip> bgmDic, seDic;

    private Dictionary<int, AudioClip> enemyDic;      //add
    private Dictionary<int, AudioClip> playerDic;
    private Dictionary<int, AudioClip> playerTalkDic;
    private object[] enemyList;
    private object[] playerList;
    private object[] playerTalkList;

    protected override void Awake()
    {
        base.Awake();
        //Load all SE & BGM files from resource folder
        bgmDic = new Dictionary<string, AudioClip>();
        seDic = new Dictionary<string, AudioClip>();

        enemyDic = new Dictionary<int, AudioClip>();        //add
        playerDic = new Dictionary<int, AudioClip>();
        playerTalkDic = new Dictionary<int, AudioClip>();

        object[] bgmList = Resources.LoadAll("Audio/BGM");
        object[] seList = Resources.LoadAll("Audio/SE");

        enemyList = Resources.LoadAll("Audio/Enemy/TakeDamage");        //add
        playerList = Resources.LoadAll("Audio/Player/TakeDamage");
        playerTalkList = Resources.LoadAll("Audio/Player/Talk");

        foreach (AudioClip bgm in bgmList)
        {
            bgmDic[bgm.name] = bgm;
        }
        foreach (AudioClip se in seList)
        {
            seDic[se.name] = se;
        }

        for (int i = 0; i < enemyList.Length; i++)        //add
        {
            enemyDic[i] = (AudioClip)enemyList[i];
        }
        for (int i = 0; i < playerList.Length; i++)
        {
            playerDic[i] = (AudioClip)playerList[i];
        }
        for (int i = 0; i < playerTalkList.Length; i++)
        {
            playerTalkDic[i] = (AudioClip)playerTalkList[i];
        }
    }

    private void Start()
    {
        AttachBGMSource.volume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, BGM_VOLUME_DEFULT);
        AttachSESource.volume = PlayerPrefs.GetFloat(SE_VOLUME_KEY, SE_VOLUME_DEFULT);

        AttachVOICESource.volume = PlayerPrefs.GetFloat(SE_VOLUME_KEY, SE_VOLUME_DEFULT);         //add
    }

    public void PlaySE(string seName, float delay = 0.0f)
    {
        if (!seDic.ContainsKey(seName))
        {
            Debug.Log(seName + "There is no SE named");
            return;
        }

        nextSEName = seName;
        Invoke(nameof(DelayPlaySE), delay);
    }

    private void DelayPlaySE()
    {
        AttachSESource.PlayOneShot(seDic[nextSEName] as AudioClip, AttachSESource.volume);
    }

    public void PlaySEAgent(string seName, float delay = 0.0f)
    {
        if (!seDic.ContainsKey(seName))
        {
            Debug.Log(seName + "There is no SE named");
            return;
        }

        nextSEName = seName;
        Invoke(nameof(DelayPlaySEAgent), delay);
    }

    private void DelayPlaySEAgent()
    {
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.BroadCast(ListenType.GET_AUDIOSOURCE, seDic[nextSEName]);
        }
    }

    public void PlayBGM(string bgmName, float fadeSpeedRate = BGM_FADE_SPEED_RATE_HIGH)
    {
        if (!bgmDic.ContainsKey(bgmName))
        {
            Debug.Log(bgmName + "There is no BGM named");
            return;
        }

        //If BGM is not currently playing, play it as is
        if (!AttachBGMSource.isPlaying)
        {
            nextBGMName = "";
            AttachBGMSource.clip = bgmDic[bgmName] as AudioClip;
            AttachBGMSource.Play();
        }
        //When a different BGM is playing, fade out the BGM that is playing before playing the next one.
        //Through when the same BGM is playing
        else if (AttachBGMSource.clip.name != bgmName)
        {
            nextBGMName = bgmName;
            FadeOutBGM(fadeSpeedRate);
        }

    }

    public void FadeOutBGM(float fadeSpeedRate = BGM_FADE_SPEED_RATE_LOW)
    {
        bgmFadeSpeedRate = fadeSpeedRate;
        isFadeOut = true;
    }

    private void Update()
    {
        if (GameManager.HasInstance)
        {
            if (GameManager.Instance.playerDeath || AttachSESource == null)
            {
                AttachSESource = transform.Find("AudioSESource").GetComponent<AudioSource>();
            }
        }

        if (!isFadeOut)
        {
            return;
        }

        //Gradually lower the volume, and when the volume reaches 0
        //return the volume and play the next song
        AttachBGMSource.volume -= Time.deltaTime * bgmFadeSpeedRate;
        if (AttachBGMSource.volume <= 0)
        {
            AttachBGMSource.Stop();
            AttachBGMSource.volume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, BGM_VOLUME_DEFULT);
            isFadeOut = false;

            if (!string.IsNullOrEmpty(nextBGMName))
            {
                PlayBGM(nextBGMName);
            }
        }
    }

    public void ChangeBGMVolume(float BGMVolume)
    {
        AttachBGMSource.volume = BGMVolume;
        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, BGMVolume);
    }

    public void ChangeSEVolume(float SEVolume)
    {
        AttachSESource.volume = SEVolume;
        PlayerPrefs.SetFloat(SE_VOLUME_KEY, SEVolume);
    }

    public void PlayEnemyTakeDamage()
    {
        enemyKey = Mathf.RoundToInt(Random.Range(0, enemyList.Length));
        AttachVOICESource.PlayOneShot(enemyDic[enemyKey] as AudioClip, 1f);
    }

    public void PlayPlayerTakeDamage()
    {
        playerKey = Mathf.RoundToInt(Random.Range(0, playerList.Length));
        AttachVOICESource.PlayOneShot(playerDic[playerKey] as AudioClip, 1f);
    }

    public void PlayPlayerTalk()
    {
        DOVirtual.DelayedCall(3f, DelayPlayerTalk);
    }

    private void DelayPlayerTalk()
    {
        talkKey = Mathf.RoundToInt(Random.Range(0, playerTalkList.Length));
        AttachVOICESource.PlayOneShot(playerTalkDic[talkKey] as AudioClip, 1f);
    }
}