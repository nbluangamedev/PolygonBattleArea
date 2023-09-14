using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class TextAlpha
{
    public TextMeshProUGUI text;
    public float minAlpha = 0.1f;
    public float maxAlpha = 0.9f;
    public float timerAlpha = 1.0f;
}

public class FlashingText : MonoBehaviour
{
    [Header("Settings")]
    public List<TextAlpha> textList = new();

    Color[] currentColor;
    TextMeshProUGUI[] thisText;
    float[] chAlpha;
    int textCount;

    void Awake()
    {
        textCount = textList.Count;

        currentColor = new Color[textCount];
        thisText = new TextMeshProUGUI[textCount];
        chAlpha = new float[textCount];

        for (int num = 0; num < textCount; num++)
        {
            thisText[num] = textList[num].text.GetComponent<TextMeshProUGUI>();
            currentColor[num] = thisText[num].color;
        }
    }

    void LateUpdate()
    {
        for (int num = 0; num < textCount; num++)
        {
            chAlpha[num] = textList[num].minAlpha + Mathf.PingPong(Time.time / textList[num].timerAlpha, textList[num].maxAlpha - textList[num].minAlpha);
            thisText[num].color = new Color(currentColor[num].r, currentColor[num].g, currentColor[num].b, Mathf.Clamp(chAlpha[num], 0.0f, 1.0f));
        }
    }
}