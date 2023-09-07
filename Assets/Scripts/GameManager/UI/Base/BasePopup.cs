using System;

public class BasePopup : BaseUIElement
{
    public override void Init()
    {
        base.Init();
        this.uiType = UIType.Popup;
    }

    public override void Hide()
    {
        base.Hide();
    }

    public override void Show(object data)
    {
        base.Show(data);
    }

    internal void Show()
    {
        throw new NotImplementedException();
    }
}