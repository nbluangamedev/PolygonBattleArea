using System;
using System.Collections.Generic;

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
        this.map = string.Empty;
        this.level = string.Empty;
        this.time = string.Empty;
        score = 0;
    }
}