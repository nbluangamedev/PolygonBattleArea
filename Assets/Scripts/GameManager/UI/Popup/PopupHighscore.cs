using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupHighscore : BasePopup
{
    public Transform entryContainer;
    public Transform entryHighscore;
    private List<Transform> highscoreEntryTransformList;

    public override void Init()
    {
        entryHighscore.gameObject.SetActive(false);
        //AddHighscoreEntry();
        //load saved entry
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        //sort rank
        for (int i = 0; i < highscores.highscoreList.Count; i++)
        {
            for (int j = 1; j < highscores.highscoreList.Count; j++)
            {
                if (highscores.highscoreList[j].score <= highscores.highscoreList[i].score)
                {                    
                    (highscores.highscoreList[i], highscores.highscoreList[j]) = (highscores.highscoreList[j], highscores.highscoreList[i]);
                }
            }
        }
        Debug.Log(PlayerPrefs.GetString("highscoreTable"));
        //show rank
        highscoreEntryTransformList = new();
        foreach (Highscore entry in highscores.highscoreList)
        {
            UpdateHighscoreList(entry, entryContainer, highscoreEntryTransformList);
        }

        base.Init();
    }

    public override void Show(object data)
    {
        entryHighscore.gameObject.SetActive(false);

        //load saved entry
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        //Debug.Log(PlayerPrefs.GetString("highscoreTable"));

        //sort rank
        for (int i = 0; i < highscores.highscoreList.Count; i++)
        {
            for (int j = 1; j < highscores.highscoreList.Count; j++)
            {
                if (highscores.highscoreList[j].score <= highscores.highscoreList[i].score)
                {
                    (highscores.highscoreList[i], highscores.highscoreList[j]) = (highscores.highscoreList[j], highscores.highscoreList[i]);
                }
            }
        }
        Debug.Log(PlayerPrefs.GetString("highscoreTable"));
        //show rank
        highscoreEntryTransformList = new();
        foreach (Highscore entry in highscores.highscoreList)
        {
            UpdateHighscoreList(entry, entryContainer, highscoreEntryTransformList);
        }

        base.Show(data);
    }

    public override void Hide()
    {
        base.Hide();
    }

    public void OnCloseButton()
    {
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

        //string rankString = rank switch
        //{
        //    1 => "1ST",
        //    2 => "2ND",
        //    3 => "3ND",
        //    _ => rank + "TH",
        //};
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
                //entryTransform.Find("Trophy").GetComponent<Image>().color = new Color(123, 60, 60);
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

    public void AddHighscoreEntry()
    {
        //create highscore entry
        //Highscore entry = new() { map = "desert", level = "hard", time = "03:28", score = 15000 };
        Highscore entry = new();

        if (GameManager.HasInstance)
        {
            //map
            if (GameManager.Instance.SelectedMap == 1)
            {
                entry.map = "DESERT";
            }
            else entry.map = "ISLAND";

            //level
            int levelScore = GameManager.Instance.Level;
            entry.level = levelScore switch
            {
                1 => "EASY",
                2 => "MEDIUM",
                3 => "HARD",
                _ => "-",
            };

            //time
            float timer = GameManager.Instance.timer;
            float minutes = Mathf.FloorToInt(timer / 60);
            float seconds = Mathf.FloorToInt(timer % 60);
            entry.time = string.Format("{0:00}:{1:00}", minutes, seconds);

            //score
            if (GameManager.Instance.Level != 0)
            {
                entry.score = (int)timer / levelScore;
            }
            else entry.score = 0;
        }

        //load saved highscores
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        //add new entry to highscores
        highscores.highscoreList.Add(entry);

        //save updated highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }

    private class Highscores
    {
        public List<Highscore> highscoreList;
    }

    [Serializable]
    private class Highscore
    {
        public string map;
        public string level;
        public string time;
        public int score;
    }
}