using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenLobby : BaseScreen
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

    public void OnLoginButton()
    {
        //string playerName = PlayerNameInput.text;
        //if (!playerName.Equals(""))
        //{
        //    PhotonNetwork.LocalPlayer.NickName = playerName;
        //    PhotonNetwork.ConnectUsingSettings();
        //}
        //else
        //{
        //    Debug.LogError("Player Name is invalid.");
        //}

        if (GameManager.HasInstance)
        {
            GameManager.Instance.ResumeGame();
        }

        if (UIManager.HasInstance)
        {
            UIManager.Instance.ShowNotify<NotifyLoadingCharacterSelection>();
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
            UIManager.Instance.ShowPopup<PopupSetting>();
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
