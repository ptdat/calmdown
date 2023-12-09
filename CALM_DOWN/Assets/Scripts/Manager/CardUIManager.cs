using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUIManager : MonoBehaviour
{
    [SerializeField] private BaseView _gameView;
    [SerializeField] private BaseView _homeView;
    [SerializeField] private BaseView _scoreView;

    void Start()
    {
        ResetUI();
    }

    void ResetUI()
    {
        _gameView.HideView();
        _scoreView.HideView();
        _homeView.ShowView();
    }

    public void ShowGameModeView()
    {
        _gameView.ShowView();
        _scoreView.HideView();
        _homeView.HideView();
    }
    public void ShowScoreView()
    {
        _gameView.HideView();
        _scoreView.ShowView();
        _homeView.HideView();
    }
    public void ShowHomeView()
    {
        _gameView.HideView();
        _scoreView.HideView();
        _homeView.ShowView();
    }
}
