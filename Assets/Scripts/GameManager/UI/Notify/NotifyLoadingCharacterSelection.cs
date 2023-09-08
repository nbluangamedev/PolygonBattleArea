using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class NotifyLoadingCharacterSelection : BaseNotify
{
    public TextMeshProUGUI loadingPercentText;
    public Slider loadingSlider;

    public override void Init()
    {
        StartCoroutine(LoadScene());
        base.Init();
    }

    public override void Show(object data)
    {
        StartCoroutine(LoadScene());
        base.Show(data);
    }

    public override void Hide()
    {
        base.Hide();
    }

    private IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("CharacterSelection");
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            loadingSlider.value = asyncOperation.progress;
            loadingPercentText.SetText($"CREATING NEW GAME: {asyncOperation.progress * 100}%");
            if (asyncOperation.progress >= 0.9f)
            {
                loadingSlider.value = 1f;
                loadingPercentText.SetText($"CREATING NEW GAME: {loadingSlider.value * 100}%");
                yield return new WaitForSeconds(2f);
                if (UIManager.HasInstance)
                {
                    UIManager.Instance.ShowOverlap<OverlapFadeCharacterSelection>();
                    UIManager.Instance.ShowScreen<ScreenCharacterSelection>();
                    this.Hide();
                    asyncOperation.allowSceneActivation = true;
                }
            }
            yield return null;
        }
    }
}