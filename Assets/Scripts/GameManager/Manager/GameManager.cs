using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : BaseManager<GameManager>
{
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

    private void Start()
    {
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.Register(ListenType.SELECTED_CHARACTER, UpdateSelectedCharacter);
            ListenerManager.Instance.Register(ListenType.SELECTED_MAP, UpdateSelectedMap);
            ListenerManager.Instance.Register(ListenType.ENEMY_COUNT, UpdateEnemyCount);
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

        //init highscore
        string jsonToLoad = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonToLoad);
        if (jsonToLoad == "" || highscores.highscoreList.Count <= 0)
        {
            Highscores highscoress = new Highscores();
            highscoress.highscoreList = new List<Highscore>()
            {
                new Highscore(){map = "DESERT", level = "EASY", time = "01:30", score = 10000},
                new Highscore(){map = "ISLAND", level = "MEDIUM", time = "02:50", score = 15000}
            };
            string jsonToSave = JsonUtility.ToJson(highscoress);
            PlayerPrefs.SetString("highscoreTable", jsonToSave);
            PlayerPrefs.Save();
        }
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
                    if (AudioManager.HasInstance)
                    {
                        AudioManager.Instance.PlaySE(AUDIO.SE_WIN);
                    }
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

    private void UpdatePlayerHealth(object value)
    {
        if (value is PlayerHealth health)
        {
            if (AudioManager.HasInstance)
            {
                if (health.CurrentHealth > 0)
                {
                    AudioManager.Instance.PlayPlayerTakeDamage();
                    playerDeath = false;
                }
                else
                {
                    AudioManager.Instance.PlaySE(AUDIO.SE_DIE2, 0.1f);
                    playerDeath = true;
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
        yield return new WaitForSeconds(3f);
        if (UIManager.HasInstance)
        {
            UIManager.Instance.ShowPopup<PopupVictory>();
        }
    }
}