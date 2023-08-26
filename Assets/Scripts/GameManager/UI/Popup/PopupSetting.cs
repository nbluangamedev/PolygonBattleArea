using UnityEngine.UI;

public class PopupSetting : BasePopup
{
    public Slider bgmSlider;
    public Slider seSlider;

    private float bgmValue;
    private float seValue;

    public override void Show(object data)
    {
        base.Show(data);
        if (AudioManager.HasInstance)
        {
            bgmValue = AudioManager.Instance.AttachBGMSource.volume;
            seValue = AudioManager.Instance.AttachSESource.volume;
        }
        bgmSlider.value = bgmValue;
        seSlider.value = seValue;
    }

    public override void Hide()
    {
        base.Hide();
    }

    public void OnClickCloseButton()
    {
        this.Hide();
    }

    public void OnBGMValueChange(float v)
    {
        bgmValue = v;
    }

    public void OnEffectValueChange(float v)
    {
        seValue = v;
    }

    public void OnApplySetting()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.ChangeBGMVolume(bgmValue);
            AudioManager.Instance.ChangeSEVolume(seValue);
        }
        this.Hide();
    }
}