using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PopupHowToPlay : BasePopup
{
    public override void Init()
    {
        base.Init();
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_SHOWPOPUP);
        }
    }

    public override void Show(object data)
    {
        base.Show(data);
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_SHOWPOPUP);
        }
    }

    public override void Hide()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_SHOWPOPUP);
        }
        base.Hide();
    }

    public void OnCloseButton()
    {
        this.Hide();
    }
}
