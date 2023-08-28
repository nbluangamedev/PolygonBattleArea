using UnityEngine;

public class ScreenHome : BaseScreen
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

    public void OnNewGameSingleButton()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.ResumeGame();
        }

        if (UIManager.HasInstance)
        {
            NotifyLoadingCharacterSelection notifyLoadingCharacterSelection = UIManager.Instance.GetExistNotify<NotifyLoadingCharacterSelection>();
            if (notifyLoadingCharacterSelection)
            {
                notifyLoadingCharacterSelection.Show(notifyLoadingCharacterSelection.gameObject);
            }
            else
            {
                UIManager.Instance.ShowNotify<NotifyLoadingCharacterSelection>();
            }
        }
        this.Hide();
    }

    public void OnSettingButton()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.IsPopupSetting = true;
        }

        if (UIManager.HasInstance)
        {
            PopupSetting popupSetting = UIManager.Instance.GetExistPopup<PopupSetting>();

            if (!popupSetting)
            {
                UIManager.Instance.ShowPopup<PopupSetting>();
            }
            else popupSetting.Show(popupSetting.gameObject);
        }
    }

    public void OnExitButton()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.EndGame();
        }
    }
}