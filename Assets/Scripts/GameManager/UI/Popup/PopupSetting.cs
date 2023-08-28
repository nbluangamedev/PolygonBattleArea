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
        }

        value = QualitySettings.GetQualityLevel();
        PlayerPrefs.SetInt("QUALITY_SETTINGS", value);
        qualityToggles[value].isOn = true;
        //Debug.Log("get quality level: " + value);
        base.Init();
    }

    public override void Show(object data)
    {
        if (AudioManager.HasInstance)
        {
            bgmValue = AudioManager.Instance.AttachBGMSource.volume;
            seValue = AudioManager.Instance.AttachSESource.volume;
            bgmSlider.value = bgmValue;
            seSlider.value = seValue;
        }
        base.Show(data);
    }

    public override void Hide()
    {
        base.Hide();
    }

    public void OnCloseButton()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.IsPopupSetting = false;
        }

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
        if (GameManager.HasInstance)
        {
            GameManager.Instance.IsPopupSetting = false;
        }

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
        this.Hide();
        //Debug.Log("Apply value qualityLevel: " + value);
    }

    public void OnBackToMenuButton()
    {

    }

    public void OnTryAgainButton()
    {

    }

    public void OnExitButton()
    {

    }
}