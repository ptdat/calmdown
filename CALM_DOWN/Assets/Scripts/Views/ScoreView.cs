using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreView : BaseView
{
    [SerializeField] private TMP_Text highsScoreText;
    [SerializeField] private Transform scoreContent;
    [SerializeField] private TMP_Text scorePrefab;
    [SerializeField] private List<TMP_Text> scoreItems;
    private ScoreData _scoreDataa;

    private void Start()
    {
        scoreItems = new List<TMP_Text>();
        // _scoreDataa = new ScoreData();
        // _scoreDataa.scores = new List<Score>();
    }

    public void ShowHighScore()
    {
        if (!CardDataScoreManager.Instance.isLoadData)
        {
            CardDataScoreManager.Instance.OpenFile();
        }
        
        _scoreDataa = CardDataScoreManager.Instance.GetScoreData();
        highsScoreText.text = $"High score: {_scoreDataa.highScore.ToString()}";
        
        ClearScoreItem();
        foreach (Score dataScore in _scoreDataa.scores)
        {
            TMP_Text score = Instantiate(scorePrefab, scoreContent);
            score.text = $"Score: {dataScore.score} - Time: {dataScore.time}";
            scoreItems.Add(score);
        }
    }

    private void ClearScoreItem()
    {
        foreach (TMP_Text item in scoreItems)
        {
            Destroy(item.gameObject);
        }
        scoreItems.Clear();
    }
}
