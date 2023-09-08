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
            float timer = GameManager.Instance.timer;
            float minutes = Mathf.FloorToInt(timer / 60);
            float seconds = Mathf.FloorToInt(timer % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            GameManager.Instance.PauseGame();
        }
        base.Init();
    }

    public override void Show(object data)
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.ReleaseCursor();
            float timer = GameManager.Instance.timer;
            float minutes = Mathf.FloorToInt(timer / 60);
            float seconds = Mathf.FloorToInt(timer % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            GameManager.Instance.PauseGame();
        }
        base.Show(data);
    }

    public override void Hide()
    {
        base.Hide();
        AddHighscoreEntry();
        if (GameManager.HasInstance)
        {
            GameManager.Instance.ResumeGame();
        }
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
    }

    public void OnBackToMenuButton()
    {
        this.Hide();
        if (UIManager.HasInstance)
        {
            ScreenGame screenGame = UIManager.Instance.GetExistScreen<ScreenGame>();
            if (screenGame)
            {
                screenGame.Hide();
            }        
            UIManager.Instance.ShowNotify<NotifyLoading>();
        }
    }

    public void OnExitButton()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.EndGame();
        }
    }

    private void AddHighscoreEntry()
    {
        //Highscore entry = new Highscore();
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