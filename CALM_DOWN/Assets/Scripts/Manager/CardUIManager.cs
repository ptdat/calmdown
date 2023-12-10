using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUIManager : Singleton<CardUIManager>
{
    [SerializeField] private GameView _gameView;
    [SerializeField] private HomeView _homeView;
    [SerializeField] private ScoreView _scoreView;

    void Start()
    {
        ResetUI();
    }

    void ResetUI()
    {
        _gameView.HideView();
        _gameView.HideWinLose();
        _scoreView.HideView();
        _homeView.ShowView();
    }

    public void ShowGameModeView()
    {
        _gameView.ShowView();
        _gameView.ShowGameMode();
        _gameView.HideWinLose();
        _scoreView.HideView();
        _homeView.HideView();
    }

    public void ShowScoreView()
    {
        _gameView.HideView();
        _gameView.HideWinLose();
        _homeView.HideView();
        _scoreView.ShowView();
        _scoreView.ShowHighScore();
    }

    public void ShowHomeView()
    {
        _gameView.HideView();
        _gameView.HideWinLose();
        _scoreView.HideView();
        _homeView.ShowView();
    }

    public void ShowWin()
    {
        _gameView.ShowView();
        _gameView.ShowWin();
    }

    public void ShowLose()
    {
        _gameView.ShowView();
        _gameView.ShowLose();
    }

}
