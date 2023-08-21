using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class NotifyLoadingGame : BaseNotify
{
    public TextMeshProUGUI loadingPercentText;
    public Slider loadingSlider;

    public override void Init()
    {
        base.Init();
        StartCoroutine(LoadScene());
    }

    public override void Show(object data)
    {
        base.Show(data);
    }

    public override void Hide()
    {
        base.Hide();
    }

    private IEnumerator LoadScene()
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
}