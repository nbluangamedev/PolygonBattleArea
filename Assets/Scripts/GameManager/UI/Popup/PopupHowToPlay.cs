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
            AudioManager.Instance.PlaySE(AUDIO.SE_SHOWPOPUP1);
        }
    }

    public override void Show(object data)
    {
        base.Show(data);
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_SHOWPOPUP1);
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

    public void OnCloseButton()
    {
        this.Hide();
    }
}
