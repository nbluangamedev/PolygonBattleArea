public class ScreenCharacterSelection : BaseScreen
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

    public void StartGame()
    {
        if (UIManager.HasInstance)
        {
            UIManager.Instance.ShowNotify<NotifyLoadingGame>();
        }
        this.Hide();
    }
}
