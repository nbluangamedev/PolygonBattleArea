using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : BaseManager<GameManager>
{
    #region Variable

    private readonly string HIGHSCORE_TABLE = "highscoreTable";

    private int selectedCharacter;
    public int SelectedCharacter
    {
        get { return selectedCharacter; }
    }

    private int selectedMap = 0;
    public int SelectedMap
    {
        get { return selectedMap; }
    }

    private int enemyCount = 0;
    public int EnemyCount
    {
        get { return enemyCount; }
        set { enemyCount = value; }
    }

    private int enemyHeadshot = 0;
    public int EnemyHeadshot
    {
        get { return enemyHeadshot; }
        set { enemyHeadshot = value; }
    }

    private bool isPopupSetting = false;
    public bool IsPopupSetting
    {
        get { return isPopupSetting; }
        set { isPopupSetting = value; }
    }

    private int level = 0;
    public int Level
    {
        set { level = value; }
        get { return level; }
    }

    public bool playerDeath = false;

    [HideInInspector] public float timer = 0;
    [HideInInspector] public int enemySpawn = 12;
    public GameObject[] weaponResetPrefabs;

    #endregion

    private void Start()
    {
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.Register(ListenType.SELECTED_CHARACTER, UpdateSelectedCharacter);
            ListenerManager.Instance.Register(ListenType.SELECTED_MAP, UpdateSelectedMap);
            ListenerManager.Instance.Register(ListenType.ENEMY_COUNT, UpdateEnemyCount);
            ListenerManager.Instance.Register(ListenType.ENEMY_HEADSHOT, UpdateEnemyHeadshot);
            ListenerManager.Instance.Register(ListenType.ON_PLAYER_DEATH, UpdatePlayerHealth);
            ListenerManager.Instance.Register(ListenType.SELECTED_LEVEL, UpdateSelectedLevel);
        }

        if (UIManager.HasInstance)
        {
            UIManager.Instance.ShowNotify<NotifyLoading>();
            NotifyLoading scr = UIManager.Instance.GetExistNotify<NotifyLoading>();
            if (scr != null)
            {
                scr.AnimationLoaddingText();
                scr.DoAnimationLoadingProgress(1, () =>
                {
                    UIManager.Instance.ShowScreen<ScreenHome>();
                    scr.Hide();
                });
            }
        }

        //reset weapon
        ResetWeaponPrefab();
    }

    private void Update()
    {
        if (UIManager.HasInstance)
        {
            ScreenGame screenGame = UIManager.Instance.GetExistScreen<ScreenGame>();
            PopupSetting popupSetting = UIManager.Instance.GetExistPopup<PopupSetting>();
            PopupSettingOnGame popupSettingOnGame = UIManager.Instance.GetExistPopup<PopupSettingOnGame>();
            PopupLose popupLose = UIManager.Instance.GetExistPopup<PopupLose>();
            PopupVictory popupVictory = UIManager.Instance.GetExistPopup<PopupVictory>();

            bool hasScreenGame = screenGame && screenGame.CanvasGroup.alpha == 1;

            if (hasScreenGame)
            {
                timer += Time.deltaTime;
                screenGame.DisplayTime(timer);
                if (enemyCount == enemySpawn)
                {
                    Invoke(nameof(PlayWinSound), 2f);
                    StartCoroutine(ShowPopupWhenVictory());
                    enemyCount = 0;
                }
            }

            bool hasPopupVictory = popupVictory && popupVictory.CanvasGroup.alpha == 1;
            bool hasPopupLose = popupLose && popupLose.CanvasGroup.alpha == 1;

            if (!hasPopupLose && !hasPopupVictory)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    isPopupSetting = !isPopupSetting;

                    if (isPopupSetting)
                    {
                        if (screenGame)
                        {
                            if (screenGame.CanvasGroup.alpha == 1)
                            {
                                ReleaseCursor();
                                UIManager.Instance.ShowPopup<PopupSettingOnGame>();
                            }
                            else
                            {
                                UIManager.Instance.ShowPopup<PopupSetting>();
                            }
                        }
                        else
                        {
                            UIManager.Instance.ShowPopup<PopupSetting>();
                        }
                    }
                    else
                    {
                        if (screenGame)
                        {
                            if (screenGame.CanvasGroup.alpha == 1)
                            {
                                LockCursor();
                                if (popupSettingOnGame)
                                {
                                    popupSettingOnGame.Hide();
                                }
                            }
                            else
                            {
                                popupSetting.Hide();
                            }
                        }
                        else
                        {
                            popupSetting.Hide();
                        }
                    }
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.Unregister(ListenType.SELECTED_CHARACTER, UpdateSelectedCharacter);
            ListenerManager.Instance.Unregister(ListenType.SELECTED_MAP, UpdateSelectedMap);
            ListenerManager.Instance.Unregister(ListenType.ENEMY_COUNT, UpdateEnemyCount);
            ListenerManager.Instance.Unregister(ListenType.ENEMY_HEADSHOT, UpdateEnemyHeadshot);
            ListenerManager.Instance.Unregister(ListenType.ON_PLAYER_DEATH, UpdatePlayerHealth);
            ListenerManager.Instance.Unregister(ListenType.SELECTED_LEVEL, UpdateSelectedLevel);
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void EndGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void ResetWeaponPrefab()
    {
        foreach (GameObject weapon in weaponResetPrefabs)
        {
            RaycastWeapon weaponReset = weapon.GetComponent<RaycastWeapon>();
            string weaponName = weaponReset.weaponName;
            switch (weaponName)
            {
                case "Pistol":
                    weaponReset.ammoCount = 12;
                    weaponReset.ammoTotal = 12;
                    break;
                case "Rifle":
                    weaponReset.ammoCount = 30;
                    weaponReset.ammoTotal = 30;
                    break;
                case "Shotgun":
                    weaponReset.ammoCount = 8;
                    weaponReset.ammoTotal = 8;
                    break;
                case "Sniper":
                    weaponReset.ammoCount = 10;
                    weaponReset.ammoTotal = 10;
                    break;
            }
        }
    }

    private void UpdateSelectedCharacter(object select)
    {
        if (select is int value)
        {
            selectedCharacter = value;
        }
    }

    private void UpdateSelectedMap(object select)
    {
        if (select is int value)
        {
            selectedMap = value;
        }
    }

    private void UpdateEnemyCount(object value)
    {
        if (value is int num)
        {
            enemyCount += num;
            if (UIManager.HasInstance)
            {
                ScreenGame screenGame = UIManager.Instance.GetExistScreen<ScreenGame>();
                if (screenGame.CanvasGroup.alpha == 1)
                {
                    screenGame.enemyCountText.text = EnemyCount.ToString();
                }
            }
        }
    }

    private void UpdateEnemyHeadshot(object value)
    {
        if (value is int num)
        {
            enemyHeadshot += num;
            if (UIManager.HasInstance)
            {
                ScreenGame screenGame = UIManager.Instance.GetExistScreen<ScreenGame>();
                if (screenGame.CanvasGroup.alpha == 1)
                {
                    screenGame.enemyHeadshotText.text = EnemyHeadshot.ToString();
                }
            }
        }
    }

    private void UpdatePlayerHealth(object value)
    {
        if (value is PlayerHealth health)
        {
            if (AudioManager.HasInstance)
            {
                if (health.CurrentHealth <= 0)
                {
                    playerDeath = true;
                    AudioManager.Instance.PlaySE(AUDIO.SE_DIE2, 0.1f);
                }
            }
        }
    }

    private void UpdateSelectedLevel(object value)
    {
        if (value is int level)
        {
            Level = level;

            enemySpawn = level switch
            {
                0 => 12,
                1 => 24,
                2 => 36,
                _ => 0
            };
        }
    }

    public void ReleaseCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void LockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private IEnumerator ShowPopupWhenVictory()
    {
        yield return new WaitForSeconds(5f);
        if (UIManager.HasInstance)
        {
            UIManager.Instance.ShowPopup<PopupVictory>();
        }
    }

    private void PlayWinSound()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_WIN);
        }
    }

    public void AddHighscoreEntry()
    {
        Highscore entry = new();

        //map
        if (SelectedMap == 0)
        {
            entry.map = "DESERT";
        }
        else entry.map = "ISLAND";

        //level
        int levelScore = Level;
        entry.level = levelScore switch
        {
            0 => "EASY",
            1 => "MEDIUM",
            2 => "HARD",
            _ => "-",
        };

        //headshot
        entry.headshot = enemyHeadshot;

        //kill
        entry.kill = enemySpawn;

        //time
        float timer = this.timer;
        float minutes = Mathf.FloorToInt(timer / 60);
        float seconds = Mathf.FloorToInt(timer % 60);
        entry.time = string.Format("{0:00}:{1:00}", minutes, seconds);

        //score
        entry.score = (EnemyHeadshot * 10) + Mathf.RoundToInt((timer * Mathf.Pow((level + 1), 2)) / enemySpawn) - ((int)seconds % 5);

        //load saved highscores
        string jsonToLoad = PlayerPrefs.GetString(HIGHSCORE_TABLE, "");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonToLoad);

        //add new entry to highscores
        highscores.highscoreList.Add(entry);

        //save updated highscores
        string jsonToSave = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString(HIGHSCORE_TABLE, jsonToSave);
        PlayerPrefs.Save();
    }
}