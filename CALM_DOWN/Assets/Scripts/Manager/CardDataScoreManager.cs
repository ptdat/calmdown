using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class CardDataScoreManager : Singleton<CardDataScoreManager>
{
    [SerializeField] private string fileName = "scoreData.txt";
    [SerializeField] private string path = "";
    [SerializeField] private ScoreData _scoreData;
    public bool isLoadData;
    void Start()
    {
        isLoadData = false;
        path = string.Concat(Application.persistentDataPath + $"/{fileName}");
        _scoreData = new ScoreData();
        _scoreData.highScore = -1;
        _scoreData.scores = new List<Score>();
         // _scoreData.scores.Add(new Score(5, 2));
         // _scoreData.scores.Add(new Score(2, 4));
         // UpdateHighScore();
         // SaveFile();
        
        if (File.Exists(path))
        {
            OpenFile();
            UpdateHighScore();
        }
        else
        {
            // save for the first time run
            SaveFile();
        }
    }

    public void OpenFile()
    {
        //Debug.Log($"Path: {path}");
        if (File.Exists(this.path))
        {
            // open file
            string data = File.ReadAllText(this.path);
            _scoreData = JsonUtility.FromJson<ScoreData>(data);
            isLoadData = true;
            //Debug.LogError($"Read high score:{_scoreData.highScore}");
        }
        else
        {
            Debug.Log("File not found!!!!");
        }
    }

    public void UpdateHighScore()
    {
        foreach (Score dataScore in _scoreData.scores)
        {
            if (_scoreData.highScore < dataScore.score)
            {
                _scoreData.highScore = dataScore.score;
            }
        }
    }
    public void SaveFile()
    {
        string data = JsonUtility.ToJson(_scoreData);
        Debug.Log($"Data to save: {data}");
        File.WriteAllText(path, data);
    }

    public ScoreData GetScoreData()
    {
        return _scoreData;
    }
}


[Serializable]
public class ScoreData
{
    public int highScore;
    public List<Score> scores;
}

[Serializable]
public class Score
{
    public float time;
    public int score;

    public Score()
    {
        
    }
    public Score(float t, int s)
    {
        time = t;
        score = s;
    }
}