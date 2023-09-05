using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class OverlapFadeLobby : BaseOverlap
{
    [SerializeField] private Image imgFade;
    [SerializeField] private Color fadeColor;

    public override void Init()
    {
        Fade(DataManager.Instance.globalConfig.loadingOverLapTime, OnFinishLobby);
        base.Init();
    }

    public override void Show(object data)
    {
        Fade(DataManager.Instance.globalConfig.loadingOverLapTime, OnFinishLobby);
        base.Show(data);
    }

    public override void Hide()
    {
        base.Hide();
    }

    public void Fade(float fadeTime, Action onFinish)
    {
        imgFade.color = fadeColor;
        SetAlpha(0);
        Sequence seq = DOTween.Sequence();
        seq.Append(this.imgFade.DOFade(1f, fadeTime));
        seq.Append(this.imgFade.DOFade(0, fadeTime));
        seq.OnComplete(() =>
        {
            onFinish?.Invoke();
        });
    }

    private void SetAlpha(float alp)
    {
        Color cl = this.imgFade.color;
        cl.a = alp;
        this.imgFade.color = cl;
    }

    private void OnFinishLobby()
    {
        //if (UIManager.HasInstance)
        //{
        //    UIManager.Instance.ShowScreen<ScreenLobby>();
        //}
        this.Hide();
    }
}
