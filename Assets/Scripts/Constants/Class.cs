using System;
using System.Collections.Generic;
using UnityEngine;

public class AIMemory
{
    public float Age
    { get { return Time.time - lastSeen; } }

    public GameObject gameObject;
    public Vector3 position;
    public Vector3 direction;
    public float distance;
    public float angle;
    public float lastSeen;
    public float score;
}

public class Highscores
{
    public List<Highscore> highscoreList;
}

[Serializable]
public class Highscore
{
    public string map;
    public string level;
    public string time;
    public int score;

    public Highscore()
    {
        this.map = "";
        this.level = "";
        this.time = "";
        score = 10000;
    }
}