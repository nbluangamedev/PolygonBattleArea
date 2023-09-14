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
            if (AudioManager.HasInstance)
            {
                AudioManager.Instance.FadeOutBGM(1f);
                AudioManager.Instance.PlayBGM(AUDIO.BGM_3ORIGINAL);
            }
        }
        if (sceneNumber == 1)
        {
            StartCoroutine(LoadScene("Small"));
            if (AudioManager.HasInstance)
            {
                AudioManager.Instance.FadeOutBGM(1f);
                AudioManager.Instance.PlayBGM(AUDIO.BGM_4MARIAN);
            }
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
            if (AudioManager.HasInstance)
            {
                AudioManager.Instance.FadeOutBGM(1f);
                AudioManager.Instance.PlayBGM(AUDIO.BGM_3ORIGINAL);
            }
        }
        if (sceneNumber == 1)
        {
            StartCoroutine(LoadScene("Small"));
            if (AudioManager.HasInstance)
            {
                AudioManager.Instance.FadeOutBGM(1f);
                AudioManager.Instance.PlayBGM(AUDIO.BGM_4MARIAN);
            }
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
            loadingPercentText.SetText($"LOADING SCENES: {Mathf.RoundToInt(asyncOperation.progress * 100)}%");
            if (asyncOperation.progress >= 0.9f)
            {
                loadingSlider.value = 1f;
                loadingPercentText.SetText($"LOADING SCENES: {loadingSlider.value * 100}%");
                yield return new WaitForSeconds(1f);
                if (UIManager.HasInstance)
                {
                    UIManager.Instance.ShowOverlap<OverlapFadeLoadingGame>();
                    UIManager.Instance.ShowScreen<ScreenGame>();
                    this.Hide();
                    asyncOperation.allowSceneActivation = true;
                }                
            }
            yield return null;
        }
    }
}