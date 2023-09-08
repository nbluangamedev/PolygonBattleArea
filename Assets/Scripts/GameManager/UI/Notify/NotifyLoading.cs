using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class NotifyLoading : BaseNotify
{
    public TextMeshProUGUI tmpLoading;
    public Slider slProgress;

    private string loadingText = "Loading";

    public override void Init()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.ResumeGame();
        }
        StartCoroutine(LoadScene());
        base.Init();
    }

    public override void Show(object data)
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.ResumeGame();
        }
        StartCoroutine(LoadScene());
        base.Show(data);
    }

    public override void Hide()
    {
        base.Hide();
    }

    public void SetProgress(float dt)
    {
        this.slProgress.value = dt;
    }

    public void AnimationLoaddingText()
    {
        DOTween.Kill(this.tmpLoading.GetInstanceID().ToString());
        Sequence seq = DOTween.Sequence();
        seq.SetId(this.tmpLoading.GetInstanceID().ToString());
        seq.AppendCallback(() =>
        {
            this.tmpLoading.text = loadingText;
        });
        seq.AppendInterval(5 / 4f);
        seq.AppendCallback(() =>
        {
            this.tmpLoading.text = loadingText + ".";
        });
        seq.AppendInterval(5 / 4f);
        seq.AppendCallback(() =>
        {
            this.tmpLoading.text = loadingText + "..";
        });
        seq.AppendInterval(5 / 4f);
        seq.AppendCallback(() =>
        {
            this.tmpLoading.text = loadingText + "...";
        });
        seq.AppendInterval(5 / 4f);
        seq.SetId(-1);
    }

    public void DoAnimationLoadingProgress(float dt, Action OnComplete = null)
    {
        DOTween.Kill(this.slProgress.GetInstanceID().ToString());
        Sequence seq = DOTween.Sequence();
        seq.SetId(this.slProgress.GetInstanceID().ToString());
        SetProgress(0);
        seq.Append(this.slProgress.DOValue(slProgress.maxValue, dt).SetEase(Ease.InQuad));
        seq.OnComplete(() =>
        {
            OnComplete?.Invoke();
        });
    }

    private IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Home");
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            AnimationLoaddingText();
            if (asyncOperation.progress >= 0.9f)
            {
                DoAnimationLoadingProgress(2f, () =>
                {
                    UIManager.Instance.ShowScreen<ScreenHome>();
                    this.Hide();
                });
                asyncOperation.allowSceneActivation = true;                
            }
            yield return null;
        }
    }
}