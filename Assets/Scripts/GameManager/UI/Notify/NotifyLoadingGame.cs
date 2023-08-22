using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class NotifyLoadingGame : BaseNotify
{
    public TextMeshProUGUI loadingPercentText;
    public Slider loadingSlider;

    private int sceneNumber;
    public override void Init()
    {
        if (GameManager.HasInstance)
        {
            sceneNumber = GameManager.Instance.SelectedMap;
        }

        base.Init();
        if (sceneNumber == 0)
        {
            StartCoroutine(LoadSceneMedium());
        }
        if(sceneNumber == 1)
        {
            StartCoroutine(LoadSceneSmall());
        }
    }

    public override void Show(object data)
    {
        base.Show(data);
    }

    public override void Hide()
    {
        base.Hide();
    }

    private IEnumerator LoadSceneMedium()
    {
        yield return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Medium");
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            loadingSlider.value = asyncOperation.progress;
            loadingPercentText.SetText($"LOADING SCENES: {asyncOperation.progress * 100}%");
            if (asyncOperation.progress >= 0.9f)
            {
                loadingSlider.value = 1f;
                loadingPercentText.SetText($"LOADING SCENES: {loadingSlider.value * 100}%");
                if (UIManager.HasInstance)
                {
                    UIManager.Instance.ShowOverlap<OverlapFadeLoadingGame>();
                }
                yield return new WaitForSeconds(1f);
                asyncOperation.allowSceneActivation = true;
                this.Hide();
            }
            yield return null;
        }
    }

    private IEnumerator LoadSceneSmall()
    {
        yield return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Small");
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            loadingSlider.value = asyncOperation.progress;
            loadingPercentText.SetText($"LOADING SCENES: {asyncOperation.progress * 100}%");
            if (asyncOperation.progress >= 0.9f)
            {
                loadingSlider.value = 1f;
                loadingPercentText.SetText($"LOADING SCENES: {loadingSlider.value * 100}%");
                if (UIManager.HasInstance)
                {
                    UIManager.Instance.ShowOverlap<OverlapFadeLoadingGame>();
                }
                yield return new WaitForSeconds(1f);
                asyncOperation.allowSceneActivation = true;
                this.Hide();
            }
            yield return null;
        }
    }
}