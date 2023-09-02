using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupHighscore : BasePopup
{
    public Transform entryContainer;
    public Transform entryHighscore;
    public TMP_InputField inputField;
    private List<Transform> highscoreEntryTransformList;

    private int removePositionHighscore;
    private int rowHighscoreDisplay = 10;

    public override void Init()
    {
        entryHighscore.gameObject.SetActive(false);
        //load saved entry
        string jsonToLoad = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonToLoad);

        if (highscores.highscoreList.Count < 10)
        {
            rowHighscoreDisplay = highscores.highscoreList.Count;
        }
        else
        {
            rowHighscoreDisplay = 10;
        }

        //bubble sort rank
        for (int i = 0; i < highscores.highscoreList.Count; i++)
        {
            for (int j = highscores.highscoreList.Count - 1; j > i; j--)
            {
                if (highscores.highscoreList[j].score < highscores.highscoreList[j - 1].score)
                {
                    (highscores.highscoreList[i], highscores.highscoreList[j]) = (highscores.highscoreList[j], highscores.highscoreList[i]);
                }
            }
        }
        Debug.Log(PlayerPrefs.GetString("highscoreTable"));
        //show rank
        highscoreEntryTransformList = new List<Transform>();

        for (int i = 0; i < rowHighscoreDisplay; i++)
        {
            UpdateHighscoreList(highscores.highscoreList[i], entryContainer, highscoreEntryTransformList);
        }

        base.Init();
    }

    public override void Show(object data)
    {
        entryHighscore.gameObject.SetActive(false);

        //load saved entry
        string jsonToLoad = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonToLoad);

        if (highscores.highscoreList.Count < 10)
        {
            rowHighscoreDisplay = highscores.highscoreList.Count;
        }
        else
        {
            rowHighscoreDisplay = 10;
        }

        //bubble sort rank
        for (int i = 0; i < highscores.highscoreList.Count; i++)
        {
            for (int j = highscores.highscoreList.Count - 1; j > i; j--)
            {
                if (highscores.highscoreList[j].score < highscores.highscoreList[j - 1].score)
                {
                    (highscores.highscoreList[i], highscores.highscoreList[j]) = (highscores.highscoreList[j], highscores.highscoreList[i]);
                }
            }
        }
        Debug.Log(PlayerPrefs.GetString("highscoreTable"));
        //show rank
        highscoreEntryTransformList = new List<Transform>();
        for (int i = 0; i < rowHighscoreDisplay; i++)
        {
            UpdateHighscoreList(highscores.highscoreList[i], entryContainer, highscoreEntryTransformList);
        }

        base.Show(data);
    }

    public override void Hide()
    {
        foreach (Transform t in entryContainer)
        {
            Destroy(t.gameObject);
        }
        base.Hide();
    }

    public void OnCloseButton()
    {
        this.Hide();
    }

    public void OnRemoveHighscore()
    {
        string jsonToLoad = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonToLoad);

        //bubble sort rank
        for (int i = 0; i < highscores.highscoreList.Count; i++)
        {
            for (int j = highscores.highscoreList.Count - 1; j > i; j--)
            {
                if (highscores.highscoreList[j].score < highscores.highscoreList[j - 1].score)
                {
                    (highscores.highscoreList[i], highscores.highscoreList[j]) = (highscores.highscoreList[j], highscores.highscoreList[i]);
                }
            }
        }

        removePositionHighscore = int.Parse(inputField.text);
        if (removePositionHighscore <= highscores.highscoreList.Count)
        {
            highscores.highscoreList.RemoveAt(removePositionHighscore - 1);
        }
        else
        {
            Debug.Log("Khong co position can xoa");
            return;
        }

        string jsonToSave = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", jsonToSave);
        PlayerPrefs.Save();

        this.Hide();
    }

    private void UpdateHighscoreList(Highscore entry, Transform container, List<Transform> transformList)
    {
        float entryHeight = 100f;
        Transform entryTransform = Instantiate(entryHighscore, container);
        RectTransform entryRect = entryTransform.GetComponent<RectTransform>();
        entryRect.anchoredPosition = new Vector2(0, -entryHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankPosition;

        switch (rank)
        {
            case 1:
                rankPosition = "1ST";
                entryTransform.Find("Trophy").GetComponent<Image>().color = new Color(255, 200, 0);
                break;
            case 2:
                rankPosition = "2ND";
                entryTransform.Find("Trophy").GetComponent<Image>().color = new Color(255, 255, 255);
                break;
            case 3:
                rankPosition = "3ND";
                break;
            default:
                rankPosition = rank + "TH";
                entryTransform.Find("Trophy").gameObject.SetActive(false);
                break;
        }

        entryTransform.Find("Position").GetComponent<TextMeshProUGUI>().text = rankPosition;
        entryTransform.Find("Map").GetComponent<TextMeshProUGUI>().text = entry.map;
        entryTransform.Find("Level").GetComponent<TextMeshProUGUI>().text = entry.level;
        entryTransform.Find("Timer").GetComponent<TextMeshProUGUI>().text = entry.time;
        entryTransform.Find("Background").gameObject.SetActive(rank % 2 == 1);


        if (rank == 1)
        {
            entryTransform.Find("Position").GetComponent<TextMeshProUGUI>().color = Color.green;
            entryTransform.Find("Map").GetComponent<TextMeshProUGUI>().color = Color.green;
            entryTransform.Find("Level").GetComponent<TextMeshProUGUI>().color = Color.green;
            entryTransform.Find("Timer").GetComponent<TextMeshProUGUI>().color = Color.green;
        }

        transformList.Add(entryTransform);
    }

    //public void AddHighscoreEntry()
    //{
    //    //create highscore entry
    //    //Highscore entry = new() { map = "desert", level = "hard", time = "03:28", score = 15000 };
    //    Highscore entry = new();

    //    if (GameManager.HasInstance)
    //    {
    //        //map
    //        if (GameManager.Instance.SelectedMap == 1)
    //        {
    //            entry.map = "DESERT";
    //        }
    //        else entry.map = "ISLAND";

    //        //level
    //        int levelScore = GameManager.Instance.Level;
    //        entry.level = levelScore switch
    //        {
    //            1 => "EASY",
    //            2 => "MEDIUM",
    //            3 => "HARD",
    //            _ => "-",
    //        };

    //        //time
    //        float timer = GameManager.Instance.timer;
    //        float minutes = Mathf.FloorToInt(timer / 60);
    //        float seconds = Mathf.FloorToInt(timer % 60);
    //        entry.time = string.Format("{0:00}:{1:00}", minutes, seconds);

    //        //score
    //        if (GameManager.Instance.Level != 0)
    //        {
    //            entry.score = (int)timer / levelScore;
    //        }
    //        else entry.score = 0;
    //    }

    //    //load saved highscores
    //    string jsonString = PlayerPrefs.GetString("highscoreTable");
    //    Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

    //    //add new entry to highscores
    //    highscores.highscoreList.Add(entry);

    //    //save updated highscores
    //    string json = JsonUtility.ToJson(highscores);
    //    PlayerPrefs.SetString("highscoreTable", json);
    //    PlayerPrefs.Save();
    //}
}