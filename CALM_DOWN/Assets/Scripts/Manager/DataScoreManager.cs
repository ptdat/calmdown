using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class DataScoreManager : Singleton<DataScoreManager>
{
    [SerializeField] private string fileName = "scoreData.txt";
    [SerializeField] private string path = "";
    [SerializeField] private ScoreData _scoreData;
    void Start()
    {
        path = string.Concat(Application.persistentDataPath + $"/{fileName}");
        _scoreData = new ScoreData();
        _scoreData.highScore = -1;
        _scoreData.scores = new List<Score>();
       
        if (File.Exists(this.path))
        {
            OpenFile(path);
        }
        else
        {
            // save for the first time run
            SaveFile(path);
        }
    }

    private void OpenFile(string path)
    {
        Debug.Log($"Path: {path}");
        if (File.Exists(this.path))
        {
            // open file
            string data = File.ReadAllText(this.path);
            _scoreData = JsonUtility.FromJson<ScoreData>(data);
            Debug.LogError($"Read high score:{_scoreData.highScore}");
        }
        else
        {
            Debug.Log("File not found!!!!");
        }
    }

    private void SaveFile(string path)
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