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
    private readonly string HIGHSCORE_TABLE = "highscoreTable";
    private int removePositionHighscore;
    private int rowHighscoreDisplay = 10;

    public override void Init()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_SHOWPOPUP1);
        }
        entryHighscore.gameObject.SetActive(false);

        //load saved entry
        string jsonToLoad = PlayerPrefs.GetString(HIGHSCORE_TABLE, "");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonToLoad);

        if (jsonToLoad != "" || highscores.highscoreList.Count > 0)
        {
            rowHighscoreDisplay = Mathf.Min(10, highscores.highscoreList.Count);

            //bubble sort rank
            for (int i = 0; i < highscores.highscoreList.Count - 1; i++)
            {
                for (int j = 0; j < highscores.highscoreList.Count - i - 1; j++)
                {
                    if (highscores.highscoreList[j].score <= highscores.highscoreList[j + 1].score)
                    {
                        (highscores.highscoreList[j], highscores.highscoreList[j + 1]) = (highscores.highscoreList[j + 1], highscores.highscoreList[j]);
                    }
                }
            }

            //show rank
            highscoreEntryTransformList = new List<Transform>();

            for (int i = 0; i < rowHighscoreDisplay; i++)
            {
                UpdateHighscoreList(highscores.highscoreList[i], entryContainer, highscoreEntryTransformList);
            }

            inputField.text = "Number";
        }
        base.Init();
    }

    public override void Show(object data)
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_SHOWPOPUP1);
        }
        entryHighscore.gameObject.SetActive(false);

        //load saved entry
        string jsonToLoad = PlayerPrefs.GetString(HIGHSCORE_TABLE, "");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonToLoad);

        if (jsonToLoad != "" || highscores.highscoreList.Count > 0)
        {
            rowHighscoreDisplay = Mathf.Min(10, highscores.highscoreList.Count);

            //bubble sort rank
            for (int i = 0; i < highscores.highscoreList.Count - 1; i++)
            {
                for (int j = 0; j < highscores.highscoreList.Count - i - 1; j++)
                {
                    if (highscores.highscoreList[j].score <= highscores.highscoreList[j + 1].score)
                    {
                        (highscores.highscoreList[j], highscores.highscoreList[j + 1]) = (highscores.highscoreList[j + 1], highscores.highscoreList[j]);
                    }
                }
            }
            //Debug.Log(PlayerPrefs.GetString("highscoreTable"));

            //show rank
            highscoreEntryTransformList = new List<Transform>();

            for (int i = 0; i < rowHighscoreDisplay; i++)
            {
                UpdateHighscoreList(highscores.highscoreList[i], entryContainer, highscoreEntryTransformList);
            }

            inputField.text = "Number";
        }
        base.Show(data);
    }

    public override void Hide()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_SHOWPOPUP1);
        }
        foreach (Transform trf in entryContainer)
        {
            Destroy(trf.gameObject);
        }
        base.Hide();
    }

    public void OnCloseButton()
    {
        this.Hide();
    }

    public void OnRemoveHighscore()
    {
        //load highscore
        string jsonToLoad = PlayerPrefs.GetString(HIGHSCORE_TABLE, "");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonToLoad);

        if (jsonToLoad != "" || highscores.highscoreList.Count > 0)
        {
            //bubble sort rank
            for (int i = 0; i < highscores.highscoreList.Count - 1; i++)
            {
                for (int j = 0; j < highscores.highscoreList.Count - i - 1; j++)
                {
                    if (highscores.highscoreList[j].score <= highscores.highscoreList[j + 1].score)
                    {
                        (highscores.highscoreList[j], highscores.highscoreList[j + 1]) = (highscores.highscoreList[j + 1], highscores.highscoreList[j]);
                    }
                }
            }

            //remove highscore
            removePositionHighscore = int.Parse(inputField.text);
            if (removePositionHighscore <= highscores.highscoreList.Count && removePositionHighscore >= 0)
            {
                highscores.highscoreList.RemoveAt(removePositionHighscore - 1);
            }
            else
            {
                Debug.Log("Khong co position can xoa");
                //return;
            }

            //save highscore
            string jsonToSave = JsonUtility.ToJson(highscores);
            PlayerPrefs.SetString(HIGHSCORE_TABLE, jsonToSave);
            PlayerPrefs.Save();
        }
        this.Hide();
    }

    private void UpdateHighscoreList(Highscore entry, Transform container, List<Transform> transformList)
    {
        float entryHeight = 100f;
        Transform entryTransform = Instantiate(entryHighscore, container);
        RectTransform entryRect = entryTransform.GetComponent<RectTransform>();
        entryRect.anchoredPosition = new Vector2(0, -entryHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        //position
        int rank = transformList.Count + 1;
        string rankPosition;
        switch (rank)
        {
            case 1:
                rankPosition = "1ST";
                entryTransform.Find("Trophy").GetComponent<Image>().color = new Color32(255, 170, 0, 255);
                entryTransform.Find("Position").GetComponent<TextMeshProUGUI>().color = Color.green;
                entryTransform.Find("Map").GetComponent<TextMeshProUGUI>().color = Color.green;
                entryTransform.Find("Level").GetComponent<TextMeshProUGUI>().color = Color.green;
                entryTransform.Find("Headshot").GetComponent<TextMeshProUGUI>().color = Color.green;
                entryTransform.Find("Split").GetComponent<TextMeshProUGUI>().color = Color.green;
                entryTransform.Find("Kill").GetComponent<TextMeshProUGUI>().color = Color.green;
                entryTransform.Find("Timer").GetComponent<TextMeshProUGUI>().color = Color.green;
                entryTransform.Find("TotalScore").GetComponent<TextMeshProUGUI>().color = Color.green;
                break;
            case 2:
                rankPosition = "2ND";
                entryTransform.Find("Trophy").GetComponent<Image>().color = new Color32(255, 255, 255, 255);
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
        
        //map
        entryTransform.Find("Map").GetComponent<TextMeshProUGUI>().text = entry.map;
        
        //level
        entryTransform.Find("Level").GetComponent<TextMeshProUGUI>().text = entry.level;

        //headshot
        entryTransform.Find("Headshot").GetComponent<TextMeshProUGUI>().text = entry.headshot.ToString();

        //kill
        entryTransform.Find("Kill").GetComponent<TextMeshProUGUI>().text = entry.kill.ToString();

        //timer
        entryTransform.Find("Timer").GetComponent<TextMeshProUGUI>().text = entry.time;

        //score
        entryTransform.Find("TotalScore").GetComponent<TextMeshProUGUI>().text = entry.score.ToString();

        //background
        entryTransform.Find("Background").gameObject.SetActive(rank % 2 == 1);

        transformList.Add(entryTransform);
    }
}