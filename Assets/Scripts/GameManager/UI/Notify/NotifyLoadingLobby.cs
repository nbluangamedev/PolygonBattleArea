using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NotifyLoadingLobby : BaseNotify
{
    public TextMeshProUGUI loadingPercentText;
    public Slider loadingSlider;

    public override void Init()
    {
        StartCoroutine(LoadScene("Lobby"));
        base.Init();
    }

    public override void Show(object data)
    {
        StartCoroutine(LoadScene("Lobby"));
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
            loadingPercentText.SetText($"CREATING NEW GAME: {asyncOperation.progress * 100}%");
            if (asyncOperation.progress >= 0.9f)
            {
                loadingSlider.value = 1f;
                loadingPercentText.SetText($"CREATING NEW GAME: {loadingSlider.value * 100}%");
                if (UIManager.HasInstance)
                {
                    UIManager.Instance.ShowOverlap<OverlapFadeLobby>();
                }
                yield return new WaitForSeconds(1f);
                asyncOperation.allowSceneActivation = true;
                this.Hide();
            }
            yield return null;
        }
    }
}
