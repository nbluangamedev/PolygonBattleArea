using TMPro;
using UnityEngine;

public class PopupVictory : BasePopup
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI scoreText;

    public override void Init()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.ReleaseCursor();
            if (UIManager.HasInstance)
            {
                ScreenGame screenGame = UIManager.Instance.GetExistScreen<ScreenGame>();
                if (screenGame.CanvasGroup.alpha == 1)
                {
                    screenGame.Hide();
                }
            }
            GameManager.Instance.PauseGame();

            float levelMap = GameManager.Instance.Level;
            float enemySpawn = GameManager.Instance.enemySpawn;
            float headshot = GameManager.Instance.EnemyHeadshot;
            float timer = GameManager.Instance.timer;
            float minutes = Mathf.FloorToInt(timer / 60);
            float seconds = Mathf.FloorToInt(timer % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            scoreText.text = ((headshot * 10) + Mathf.RoundToInt((timer * Mathf.Pow((levelMap + 1), 2)) / enemySpawn) - ((int)seconds % 10)).ToString();
        }
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_SHOWPOPUP1);
        }
        base.Init();
    }

    public override void Show(object data)
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.ReleaseCursor();
            if (UIManager.HasInstance)
            {
                ScreenGame screenGame = UIManager.Instance.GetExistScreen<ScreenGame>();
                if (screenGame.CanvasGroup.alpha == 1)
                {
                    screenGame.Hide();
                }
            }
            GameManager.Instance.PauseGame();

            float levelMap = GameManager.Instance.Level;
            float enemySpawn = GameManager.Instance.enemySpawn;
            float headshot = GameManager.Instance.EnemyHeadshot;
            float timer = GameManager.Instance.timer;
            float minutes = Mathf.FloorToInt(timer / 60);
            float seconds = Mathf.FloorToInt(timer % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            scoreText.text = ((headshot * 10) + Mathf.RoundToInt((timer * Mathf.Pow((levelMap + 1), 2)) / enemySpawn) - ((int)seconds % 10)).ToString();
        }
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_SHOWPOPUP1);
        }
        base.Show(data);
    }

    public override void Hide()
    {
        base.Hide();
        if (GameManager.HasInstance)
        {
            GameManager.Instance.ResumeGame();
            GameManager.Instance.AddHighscoreEntry();
        }
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_SHOWPOPUP1);
        }
    }

    public void OnTryAgainButton()
    {
        this.Hide();
        if (UIManager.HasInstance)
        {
            ScreenGame screenGame = UIManager.Instance.GetExistScreen<ScreenGame>();
            if (screenGame)
            {
                screenGame.Hide();
            }
            UIManager.Instance.ShowNotify<NotifyLoadingCharacterSelection>();
        }
    }

    public void OnBackToMenuButton()
    {
        this.Hide();
        if (UIManager.HasInstance)
        {
            ScreenGame screenGame = UIManager.Instance.GetExistScreen<ScreenGame>();
            if (screenGame)
            {
                screenGame.Hide();
            }
            UIManager.Instance.ShowNotify<NotifyLoading>();
        }
    }

    public void OnExitButton()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.EndGame();
        }
    }
}