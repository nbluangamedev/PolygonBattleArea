using TMPro;
using UnityEngine;

public class PopupVictory : BasePopup
{
    [SerializeField] private TextMeshProUGUI timerText;

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
            float timer = GameManager.Instance.timer;
            float minutes = Mathf.FloorToInt(timer / 60);
            float seconds = Mathf.FloorToInt(timer % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            GameManager.Instance.PauseGame();
        }
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_SHOWPOPUP);
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
        }
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_SHOWPOPUP);
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
            AudioManager.Instance.PlaySE(AUDIO.SE_SHOWPOPUP);
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