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
}
