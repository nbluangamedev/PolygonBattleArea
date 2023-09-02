using TMPro;
using UnityEngine;

public class PopupVictory : BasePopup
{
    [SerializeField] private TextMeshProUGUI timerText;

    public override void Init()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.ReleaseCursor();
            GameManager.Instance.PauseGame();
            float timer = GameManager.Instance.timer;
            float minutes = Mathf.FloorToInt(timer / 60);
            float seconds = Mathf.FloorToInt(timer % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        AddHighscoreEntry();
        base.Init();
    }

    public override void Show(object data)
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.ReleaseCursor();
            GameManager.Instance.PauseGame();
            float timer = GameManager.Instance.timer;
            float minutes = Mathf.FloorToInt(timer / 60);
            float seconds = Mathf.FloorToInt(timer % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        AddHighscoreEntry();
        base.Show(data);
    }

    public override void Hide()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.ResumeGame();
            GameManager.Instance.EnemyCount = 0;
        }        
        base.Hide();
    }

    public void OnTryAgainButton()
    {
        this.Hide();
        if (UIManager.HasInstance)
        {
            ScreenGame screenGame = UIManager.Instance.GetExistScreen<ScreenGame>();

            if (screenGame)
            {
                screenGame.Hide();
            }

            UIManager.Instance.ShowNotify<NotifyLoadingCharacterSelection>();
        }

        if (CameraManager.HasInstance)
        {
            CameraManager.Instance.DisableKillCam();
        }

        if (GameManager.HasInstance)
        {
            GameManager.Instance.LoadScene("CharacterSelection");
        }
    }

    public void OnBackToMenuButton()
    {
        this.Hide();
        if (CameraManager.HasInstance)
        {
            CameraManager.Instance.DisableKillCam();
        }

        if (UIManager.HasInstance)
        {
            ScreenGame screenGame = UIManager.Instance.GetExistScreen<ScreenGame>();
            if (screenGame)
            {
                screenGame.Hide();
            }
        }

        if (GameManager.HasInstance)
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
            GameManager.Instance.LoadScene("Home");
        }
    }

    public void OnExitButton()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.EndGame();
        }
    }

    public void AddHighscoreEntry()
    {
        Highscore entry = new Highscore();

        if (GameManager.HasInstance)
        {
            //map
            if (GameManager.Instance.SelectedMap == 0)
            {
                entry.map = "DESERT";
            }
            else entry.map = "ISLAND";

            //level
            int levelScore = GameManager.Instance.Level;
            entry.level = levelScore switch
            {
                0 => "EASY",
                1 => "MEDIUM",
                2 => "HARD",
                _ => "-",
            };

            //time
            float timer = GameManager.Instance.timer;
            float minutes = Mathf.FloorToInt(timer / 60);
            float seconds = Mathf.FloorToInt(timer % 60);
            entry.time = string.Format("{0:00}:{1:00}", minutes, seconds);

            //score
            entry.score = (int)(timer / Mathf.Pow(levelScore + 1, 2));
        }

        //load saved highscores
        string jsonToLoad = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonToLoad);

        //add new entry to highscores
        highscores.highscoreList.Add(entry);

        //save updated highscores
        string jsonToSave = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", jsonToSave);
        PlayerPrefs.Save();
    }
}