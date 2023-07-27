using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class OverlapFade : BaseOverlap
{
    [SerializeField]
    private Image imgFade;
    [SerializeField]
    private Color fadeColor;

    public override void Init()
    {
        base.Init();
        Fade(DataManager.Instance.globalConfig.loadingOverLapTime, OnFinish);
    }

    public override void Show(object data)
    {
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

    private void OnFinish()
    {
        if (UIManager.HasInstance)
        {
            UIManager.Instance.ShowScreen<ScreenGame>();
        }
        this.Hide();
    }
}