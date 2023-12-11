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
    [SerializeField] private bool isTestData = true;
    void Start()
    {
        isLoadData = false;
        path = string.Concat(Application.persistentDataPath + $"/{fileName}");
        _scoreData = new ScoreData();
        _scoreData.shortestTime = -1;
        _scoreData.scores = new List<Score>();
        
        if (isTestData)
        {
            _scoreData.scores.Add(new Score(25.2f));
            _scoreData.scores.Add(new Score(14f));
            UpdateShortestTime();
            SaveFile();
        }

        if (File.Exists(path))
        {
            OpenFile();
            UpdateShortestTime();
        }
        else
        {
            // save for the first time run
            SaveFile();
        }
    }

    public void AddNewTime(float playTime)
    {
        _scoreData.scores.Add(new Score(playTime));
        UpdateShortestTime();
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
        }
        else
        {
            Debug.Log("File not found!!!!");
        }
    }

    public void UpdateShortestTime()
    {
        if (_scoreData.shortestTime < 0 && _scoreData.scores.Count > 0)
            _scoreData.shortestTime = _scoreData.scores[0].time;
        foreach (Score dataScore in _scoreData.scores)
        {
            if (dataScore.time < _scoreData.shortestTime)
            {
                _scoreData.shortestTime = dataScore.time;
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
    public float shortestTime;
    public List<Score> scores;
}

[Serializable]
public class Score
{
    public float time;
    public Score()
    {
        
    }
    public Score(float t)
    {
        time = t;
    }
}