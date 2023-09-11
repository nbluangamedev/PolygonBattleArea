public class PopupLose : BasePopup
{
    public override void Init()
    {
        base.Init();

        if (GameManager.HasInstance)
        {
            GameManager.Instance.PauseGame();
        }
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_SHOWPOPUP);
        }
    }

    public override void Show(object data)
    {
        base.Show(data);
        if (GameManager.HasInstance)
        {
            GameManager.Instance.PauseGame();
        }
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_SHOWPOPUP);
        }
    }

    public override void Hide()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.ResumeGame();
        }
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_SHOWPOPUP);
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