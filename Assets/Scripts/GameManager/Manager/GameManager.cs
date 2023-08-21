using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : BaseManager<GameManager>
{
    private int selectedCharacter;

    public int SelectedCharacter
    {
        get { return selectedCharacter; }
    }

    private void Start()
    {
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.Register(ListenType.SELECTED_CHARACTER, UpdateSelectdCharacter);
        }

        if (UIManager.HasInstance)
        {
            UIManager.Instance.ShowNotify<NotifyLoading>();
            NotifyLoading scr = UIManager.Instance.GetExistNotify<NotifyLoading>();
            if (scr != null)
            {
                scr.AnimationLoaddingText();
                scr.DoAnimationLoadingProgress(2, () =>
                {
                    UIManager.Instance.ShowScreen<ScreenHome>();
                    scr.Hide();
                });
            }
        }
    }

    private void OnDestroy()
    {
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.Unregister(ListenType.SELECTED_CHARACTER, UpdateSelectdCharacter);
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void UpdateSelectdCharacter(object select)
    {
        if (select is int value)
        {
            selectedCharacter = value;
        }
    }
}