using UnityEngine;

public class ScreenHome : BaseScreen
{
    public override void Init()
    {
        base.Init();
    }

    public override void Show(object data)
    {
        base.Show(data);
    }

    public override void Hide()
    {
        base.Hide();
    }

    public void OnNewGameSingleButton()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.ResumeGame();            
        }

        if (UIManager.HasInstance)
        {
            UIManager.Instance.ShowNotify<NotifyLoadingCharacterSelection>();
        }

        this.Hide();
    }

    public void OnNewGameMultiplayerButton()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.ResumeGame();
            GameManager.Instance.LoadScene("Lobby");
        }

        //if (UIManager.HasInstance)
        //{
        //    UIManager.Instance.ShowNotify<NotifyLoadingLobby>();
        //}
        Debug.Log("multi");
        this.Hide();
    }

    public void OnSettingButton()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.IsPopupSetting = true;
        }

        if (UIManager.HasInstance)
        {
            UIManager.Instance.ShowPopup<PopupSetting>();
        }
    }

    public void OnHighscoreButton()
    {
        if (UIManager.HasInstance)
        {
            PopupHighscore popupHighscore = UIManager.Instance.GetExistPopup<PopupHighscore>();
            if (popupHighscore)
            {
                if (popupHighscore.CanvasGroup.alpha == 1)
                {
                    popupHighscore.Hide();
                }
                else UIManager.Instance.ShowPopup<PopupHighscore>();
            }
            else UIManager.Instance.ShowPopup<PopupHighscore>();
        }
    }

    public void OnHowToPlayButton()
    {
        if (UIManager.HasInstance)
        {
            PopupHowToPlay popupHowToPlay = UIManager.Instance.GetExistPopup<PopupHowToPlay>();
            if (popupHowToPlay)
            {
                if (popupHowToPlay.CanvasGroup.alpha == 1)
                {
                    popupHowToPlay.Hide();
                }
                else UIManager.Instance.ShowPopup<PopupHowToPlay>();
            }
            else UIManager.Instance.ShowPopup<PopupHowToPlay>();
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