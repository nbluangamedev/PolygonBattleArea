public class ScreenCharacterSelection : BaseScreen
{
    public override void Init()
    {
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
        base.Hide();
    }

    public void NextCharacter()
    {
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.BroadCast(ListenType.CHARACTER_SELECTION, 1);
        }
    }

    public void PreviousCharacter()
    {
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.BroadCast(ListenType.CHARACTER_SELECTION, 0);
        }
    }

    public void SelectedMap(int value)
    {
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.BroadCast(ListenType.SELECTED_MAP, value);
        }
    }

    public void PlayGame()
    {
        if (UIManager.HasInstance)
        {
            NotifyLoadingGame notifyLoadingGame = UIManager.Instance.GetExistNotify<NotifyLoadingGame>();
            if (notifyLoadingGame)
            {
                notifyLoadingGame.Show(notifyLoadingGame.gameObject);
            }
            else
            {
                UIManager.Instance.ShowNotify<NotifyLoadingGame>();
            }
        }
        if (GameManager.HasInstance)
        {
            GameManager.Instance.ResumeGame();
        }
        this.Hide();
    }
}