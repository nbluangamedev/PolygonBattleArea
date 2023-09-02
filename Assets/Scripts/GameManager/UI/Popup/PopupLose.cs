public class PopupLose : BasePopup
{
    public override void Init()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.PauseGame();
        }
        base.Init();
    }

    public override void Show(object data)
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.PauseGame();
        }
        base.Show(data);
    }

    public override void Hide()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.ResumeGame();
        }
        base.Hide();
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

        if (CameraManager.HasInstance)
        {
            CameraManager.Instance.DisableKillCam();
        }

        if (GameManager.HasInstance)
        {
            GameManager.Instance.LoadScene("Home");
        }
    }

    public void OnBackToMenuButton()
    {
        this.Hide();
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
        }

        if (GameManager.HasInstance)
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
            GameManager.Instance.LoadScene("Home");
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