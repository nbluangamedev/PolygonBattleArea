using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : BaseManager<GameManager>
{
    private int selectedCharacter;
    private int selectedMap;

    public int SelectedCharacter
    {
        get { return selectedCharacter; }
    }

    public int SelectedMap
    {
        get { return selectedMap; }
    }

    private void Start()
    {
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.Register(ListenType.SELECTED_CHARACTER, UpdateSelectedCharacter);
            ListenerManager.Instance.Register(ListenType.SELECTED_MAP, UpdateSelectedMap);
        }

        if (UIManager.HasInstance)
        {
            UIManager.Instance.ShowNotify<NotifyLoading>();
            NotifyLoading scr = UIManager.Instance.GetExistNotify<NotifyLoading>();
            if (scr != null)
            {
                scr.AnimationLoaddingText();
                scr.DoAnimationLoadingProgress(1, () =>
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
            ListenerManager.Instance.Unregister(ListenType.SELECTED_CHARACTER, UpdateSelectedCharacter);
            ListenerManager.Instance.Unregister(ListenType.SELECTED_MAP, UpdateSelectedMap);
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void UpdateSelectedCharacter(object select)
    {
        if (select is int value)
        {
            selectedCharacter = value;
        }
    }

    private void UpdateSelectedMap(object select)
    {
        if (select is int value)
        {
            selectedMap = value;
        }
    }
}