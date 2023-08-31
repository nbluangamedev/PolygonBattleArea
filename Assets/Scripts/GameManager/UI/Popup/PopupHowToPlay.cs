using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PopupHowToPlay : BasePopup
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

    public void OnCloseButton()
    {
        this.Hide();
    }
}
