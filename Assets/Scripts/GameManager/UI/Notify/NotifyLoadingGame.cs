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
        if (sceneNumber == 0)
        {
            StartCoroutine(LoadScene("Medium"));
        }
        if(sceneNumber == 1)
        {
            StartCoroutine(LoadScene("Small"));
        }
        //Debug.Log("Init sceneNumber " + sceneNumber);
        base.Init();
    }

    public override void Show(object data)
    {
        if (GameManager.HasInstance)
        {
            sceneNumber = GameManager.Instance.SelectedMap;
        }
        if (sceneNumber == 0)
        {
            StartCoroutine(LoadScene("Medium"));
        }
        if (sceneNumber == 1)
        {
            StartCoroutine(LoadScene("Small"));
        }
        //Debug.Log("Show sceneNumber" + sceneNumber);
        base.Show(data);
    }

    public override void Hide()
    {
        base.Hide();
    }

    private IEnumerator LoadScene(string sceneName)
    {
        yield return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
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
                if (AudioManager.HasInstance)
                {
                    AudioManager.Instance.FadeOutBGM(1f);
                }
                yield return new WaitForSeconds(1f);
                asyncOperation.allowSceneActivation = true;
                this.Hide();
            }
            yield return null;
        }
    }
}