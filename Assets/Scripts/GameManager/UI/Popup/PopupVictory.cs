using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PopupVictory : BasePopup
{
    [SerializeField] private TextMeshProUGUI timerText;

    public override void Init()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.PauseGame();
            timerText.text = GameManager.Instance.timer.ToString();
        }
        base.Init();
    }

    public override void Show(object data)
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.PauseGame();
            timerText.text = GameManager.Instance.timer.ToString();
        }
        base.Show(data);
    }

    public override void Hide()
    {
        base.Hide();
    }

    public void OnTryAgainButton()
    {
        if (UIManager.HasInstance)
        {
            ScreenGame screenGame = UIManager.Instance.GetExistScreen<ScreenGame>();
            ScreenCharacterSelection screenCharacterSelection = UIManager.Instance.GetExistScreen<ScreenCharacterSelection>();
            NotifyLoadingCharacterSelection notifyLoadingCharacterSelection = UIManager.Instance.GetExistNotify<NotifyLoadingCharacterSelection>();

            if (screenGame)
            {
                screenGame.Hide();
            }

            if (notifyLoadingCharacterSelection)
            {
                notifyLoadingCharacterSelection.Hide();
            }

            if (screenCharacterSelection)
            {
                screenCharacterSelection.Show(screenCharacterSelection.gameObject);
            }
            else UIManager.Instance.ShowScreen<ScreenCharacterSelection>();
        }

        if (GameManager.HasInstance)
        {
            GameManager.Instance.LoadScene("CharacterSelection");
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
            ScreenGame screenGame = UIManager.Instance.GetExistScreen<ScreenGame>();
            ScreenCharacterSelection screenCharacterSelection = UIManager.Instance.GetExistScreen<ScreenCharacterSelection>();
            NotifyLoading notifyLoading = UIManager.Instance.GetExistNotify<NotifyLoading>();

            if (screenGame)
            {
                screenGame.Hide();
            }

            if (notifyLoading)
            {
                notifyLoading.AnimationLoaddingText();
                notifyLoading.DoAnimationLoadingProgress(1, () =>
                {
                    UIManager.Instance.ShowScreen<ScreenHome>();
                    notifyLoading.Hide();
                });
            }
            else
            {
                UIManager.Instance.ShowNotify<NotifyLoading>();
                NotifyLoading ntfLoading = UIManager.Instance.GetExistNotify<NotifyLoading>();
                if (ntfLoading)
                {
                    ntfLoading.AnimationLoaddingText();
                    ntfLoading.DoAnimationLoadingProgress(1, () =>
                    {
                        UIManager.Instance.ShowScreen<ScreenHome>();
                        ntfLoading.Hide();
                    });
                }
            }
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