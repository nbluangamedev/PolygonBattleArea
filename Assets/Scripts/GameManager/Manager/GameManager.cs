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

    private int selectedMap;
    public int SelectedMap
    {
        get { return selectedMap; }
    }

    private int enemyCount;
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

    private bool playerDeath = false;
    public bool PlayerDeath
    {
        get { return playerDeath; }
        set { playerDeath = value; }
    }

    private void Start()
    {
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.Register(ListenType.SELECTED_CHARACTER, UpdateSelectedCharacter);
            ListenerManager.Instance.Register(ListenType.SELECTED_MAP, UpdateSelectedMap);
            ListenerManager.Instance.Register(ListenType.ENEMY_COUNT, UpdateEnemyRemain);
            ListenerManager.Instance.Register(ListenType.ON_PLAYER_DEATH, UpdatePlayerHealth);
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

            if (enemyCount == 20)
            {
                //Debug.Log("you win");
                UIManager.Instance.ShowPopup<PopupVictory>();
            }
        }
    }

    private void Update()
    {
        if (UIManager.HasInstance)
        {
            PopupLose popupLose = UIManager.Instance.GetExistPopup<PopupLose>();

            if (!popupLose || popupLose.CanvasGroup.alpha == 0)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    isPopupSetting = !isPopupSetting;
                    Debug.Log("pop setting enable " + IsPopupSetting);
                    ScreenGame screenGame = UIManager.Instance.GetExistScreen<ScreenGame>();
                    PopupSettingOnGame popupSettingOnGame = UIManager.Instance.GetExistPopup<PopupSettingOnGame>();
                    PopupSetting popupSetting = UIManager.Instance.GetExistPopup<PopupSetting>();

                    if (isPopupSetting)
                    {
                        if (screenGame)
                        {
                            if (screenGame.CanvasGroup.alpha == 1)
                            {
                                ReleaseCursor();
                                if (!popupSettingOnGame)
                                {
                                    UIManager.Instance.ShowPopup<PopupSettingOnGame>();
                                }
                                else popupSettingOnGame.Show(popupSettingOnGame.gameObject);
                            }
                            else
                            {
                                ShowPopupSetting(popupSetting);
                            }
                        }
                        else
                        {
                            ShowPopupSetting(popupSetting);
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
                                HidePopupSetting(popupSetting);
                            }
                        }
                        else
                        {
                            HidePopupSetting(popupSetting);
                        }
                    }
                }
            }
        }
    }

    private void HidePopupSetting(PopupSetting popupSetting)
    {
        if (popupSetting)
        {
            popupSetting.Hide();
        }
    }

    private void ShowPopupSetting(PopupSetting popupSetting)
    {
        if (!popupSetting)
        {
            UIManager.Instance.ShowPopup<PopupSetting>();
        }
        else popupSetting.Show(popupSetting.gameObject);
    }

    private void OnDestroy()
    {
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.Unregister(ListenType.SELECTED_CHARACTER, UpdateSelectedCharacter);
            ListenerManager.Instance.Unregister(ListenType.SELECTED_MAP, UpdateSelectedMap);
            ListenerManager.Instance.Unregister(ListenType.ENEMY_COUNT, UpdateEnemyRemain);
            ListenerManager.Instance.Unregister(ListenType.ON_PLAYER_DEATH, UpdatePlayerHealth);
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

    private void UpdateEnemyRemain(object value)
    {
        if (value is int num)
        {
            enemyCount += num;
            Debug.Log("enemy count: " + enemyCount);
        }
    }

    private void UpdatePlayerHealth(object value)
    {
        if (value is PlayerHealth health)
        {
            playerDeath = health.CurrentHealth <= 0;
            Debug.Log("player death: " + playerDeath);
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
}