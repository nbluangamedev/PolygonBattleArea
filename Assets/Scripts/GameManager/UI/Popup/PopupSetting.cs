using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PopupSetting : BasePopup
{
    public Slider bgmSlider;
    public Slider seSlider;

    public RenderPipelineAsset[] qualityLevels;
    public Toggle[] qualityToggles;

    private float bgmValue;
    private float seValue;
    private int value;

    public override void Init()
    {
        if (AudioManager.HasInstance)
        {
            bgmValue = AudioManager.Instance.AttachBGMSource.volume;
            seValue = AudioManager.Instance.AttachSESource.volume;
            bgmSlider.value = bgmValue;
            seSlider.value = seValue;
            AudioManager.Instance.PlaySE(AUDIO.SE_SHOWPOPUP1);
        }

        value = QualitySettings.GetQualityLevel();
        PlayerPrefs.SetInt("QUALITY_SETTINGS", value);
        qualityToggles[value].isOn = true;

        base.Init();
    }

    public override void Show(object data)
    {
        base.Show(data);
        if (AudioManager.HasInstance)
        {
            bgmValue = AudioManager.Instance.AttachBGMSource.volume;
            seValue = AudioManager.Instance.AttachSESource.volume;
            bgmSlider.value = bgmValue;
            seSlider.value = seValue;
            AudioManager.Instance.PlaySE(AUDIO.SE_SHOWPOPUP1);
        }
    }

    public override void Hide()
    {
        base.Hide();
        if (GameManager.HasInstance)
        {
            GameManager.Instance.IsPopupSetting = false;
        }
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_SHOWPOPUP1);
        }
    }

    public void OnCloseButton()
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

    public void OnToggleSelect()
    {
        qualityToggles[0].onValueChanged.AddListener((isSelected) =>
        {
            if (isSelected)
            {
                value = (int)QualitySetting.LOW;
            }
        });

        qualityToggles[1].onValueChanged.AddListener((isSelected) =>
        {
            if (isSelected)
            {
                value = (int)QualitySetting.MEDIUM;
            }
        });

        qualityToggles[2].onValueChanged.AddListener((isSelected) =>
        {
            if (isSelected)
            {
                value = (int)QualitySetting.HIGH;
            }
        });
    }

    public void OnApplyButton()
    {
        this.Hide();
        if (AudioManager.HasInstance)
        {
            if (bgmValue != AudioManager.Instance.AttachBGMSource.volume)
            {
                AudioManager.Instance.ChangeBGMVolume(bgmValue);
            }

            if (seValue != AudioManager.Instance.AttachSESource.volume)
            {
                AudioManager.Instance.ChangeSEVolume(seValue);
            }
        }

        if (value != QualitySettings.GetQualityLevel())
        {
            QualitySettings.SetQualityLevel(value);
            QualitySettings.renderPipeline = qualityLevels[value];
            PlayerPrefs.SetInt("QUALITY_SETTINGS", value);
        }
    }

    public void OnInstagramButton()
    {
        Application.OpenURL("https://www.instagram.com/nbluangamedev");
    }

    public void OnFacebookButton()
    {
        Application.OpenURL("https://www.facebook.com/nbluan89");
        this.Hide();
    }

    public void OnTwitterButton()
    {
        Application.OpenURL("https://twitter.com/nbluan89");
    }
}