using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PopupSettingOnGame : BasePopup
{
    public Slider bgmSlider;
    public Slider seSlider;
    public Slider mouseSlider;

    public RenderPipelineAsset[] qualityLevels;
    public Toggle[] qualityToggles;

    private float bgmValue;
    private float seValue;
    private float mouseValue = 300f;
    private int qualityValue;

    public override void Init()
    {
        if (AudioManager.HasInstance)
        {
            bgmValue = AudioManager.Instance.AttachBGMSource.volume;
            seValue = AudioManager.Instance.AttachSESource.volume;
            bgmSlider.value = bgmValue;
            seSlider.value = seValue;
            mouseSlider.value = mouseValue;
            AudioManager.Instance.PlaySE(AUDIO.SE_SHOWPOPUP);
        }
        if (GameManager.HasInstance)
        {
            GameManager.Instance.PauseGame();
        }
        qualityValue = QualitySettings.GetQualityLevel();
        PlayerPrefs.SetInt("QUALITY_SETTINGS", qualityValue);
        qualityToggles[qualityValue].isOn = true;

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
            AudioManager.Instance.PlaySE(AUDIO.SE_SHOWPOPUP);
        }
        if (GameManager.HasInstance)
        {
            GameManager.Instance.PauseGame();
        }
        qualityValue = QualitySettings.GetQualityLevel();
        PlayerPrefs.SetInt("QUALITY_SETTINGS", qualityValue);
        qualityToggles[qualityValue].isOn = true;
        base.Show(data);
    }

    public override void Hide()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.IsPopupSetting = false;
            GameManager.Instance.ResumeGame();
        }
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_SHOWPOPUP);
        }
        base.Hide();
    }

    public void OnCloseButton()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.LockCursor();
            GameManager.Instance.ResumeGame();
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

    public void OnMouseSpeedValueChange(float v)
    {
        mouseValue = v;
    }

    public void OnToggleSelect()
    {
        qualityToggles[0].onValueChanged.AddListener((isSelected) =>
        {
            if (isSelected)
            {
                qualityValue = (int)QualitySetting.LOW;
            }
        });

        qualityToggles[1].onValueChanged.AddListener((isSelected) =>
        {
            if (isSelected)
            {
                qualityValue = (int)QualitySetting.MEDIUM;
            }
        });

        qualityToggles[2].onValueChanged.AddListener((isSelected) =>
        {
            if (isSelected)
            {
                qualityValue = (int)QualitySetting.HIGH;
            }
        });
    }

    public void OnApplyButton()
    {
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

        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.BroadCast(ListenType.UPDATE_MOUSE_SPEED, mouseValue);
        }

        if (qualityValue != QualitySettings.GetQualityLevel())
        {
            QualitySettings.SetQualityLevel(qualityValue);
            QualitySettings.renderPipeline = qualityLevels[qualityValue];
            PlayerPrefs.SetInt("QUALITY_SETTINGS", qualityValue);
        }

        if (GameManager.HasInstance)
        {
            GameManager.Instance.LockCursor();
            GameManager.Instance.ResumeGame();
        }
        this.Hide();
    }

    public void OnBackToMenuButton()
    {
        if (CameraManager.HasInstance)
        {
            CameraManager.Instance.DisableKillCam();
        }

        if (UIManager.HasInstance)
        {
            UIManager.Instance.HideAllPopups();

            ScreenGame screenGame = UIManager.Instance.GetExistScreen<ScreenGame>();
            if (screenGame.CanvasGroup.alpha == 1)
            {
                screenGame.Hide();
            }
            UIManager.Instance.ShowNotify<NotifyLoading>();
        }
        this.Hide();
    }

    public void OnTryAgainButton()
    {
        if (UIManager.HasInstance)
        {
            ScreenGame screenGame = UIManager.Instance.GetExistScreen<ScreenGame>();
            if (screenGame)
            {
                screenGame.Hide();
            }
            UIManager.Instance.ShowNotify<NotifyLoadingCharacterSelection>();
        }
        this.Hide();
    }

    public void OnExitButton()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.EndGame();
        }
    }
}