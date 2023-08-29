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
            GameManager.Instance.EnemyCount = 0;
        }

        if (UIManager.HasInstance)
        {
            UIManager.Instance.ShowNotify<NotifyLoadingCharacterSelection>();
        }
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

    public void OnExitButton()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.EndGame();
        }
    }
}