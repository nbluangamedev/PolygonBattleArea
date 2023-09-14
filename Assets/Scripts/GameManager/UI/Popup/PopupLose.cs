public class PopupLose : BasePopup
{
    public override void Init()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_SHOWPOPUP1);
        }
        if (UIManager.HasInstance)
        {
            ScreenGame screenGame = UIManager.Instance.GetExistScreen<ScreenGame>();
            if (screenGame.CanvasGroup.alpha == 1)
            {
                screenGame.Hide();
            }
        }
        base.Init();
        if (GameManager.HasInstance)
        {            
            GameManager.Instance.PauseGame();
        }
    }

    public override void Show(object data)
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_SHOWPOPUP1);
        }
        if (UIManager.HasInstance)
        {
            ScreenGame screenGame = UIManager.Instance.GetExistScreen<ScreenGame>();
            if (screenGame.CanvasGroup.alpha == 1)
            {
                screenGame.Hide();
            }
        }
        base.Show(data);
        if (GameManager.HasInstance)
        {
            GameManager.Instance.PauseGame();
        }
    }

    public override void Hide()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_SHOWPOPUP1);
        }
        base.Hide();
    }

    public void OnTryAgainButton()
    {
        if (UIManager.HasInstance)
        {
            ScreenGame screenGame = UIManager.Instance.GetExistScreen<ScreenGame>();
            if (screenGame)
            {
                screenGame.Hide();
            }
            UIManager.Instance.ShowNotify<NotifyLoadingCharacterSelection>();
        }

        if (CameraManager.HasInstance)
        {
            CameraManager.Instance.DisableKillCam();
        }
        this.Hide();
    }

    public void OnBackToMenuButton()
    {
        if (CameraManager.HasInstance)
        {
            CameraManager.Instance.DisableKillCam();
        }

        if (UIManager.HasInstance)
        {
            ScreenGame screenGame = UIManager.Instance.GetExistScreen<ScreenGame>();
            if (screenGame)
            {
                screenGame.Hide();
            }        
            UIManager.Instance.ShowNotify<NotifyLoading>();
        }
        this.Hide();
    }

    public void OnExitButton()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.EndGame();
        }
    }
}