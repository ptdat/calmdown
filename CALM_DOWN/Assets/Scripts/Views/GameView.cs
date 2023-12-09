using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameView : BaseView
{
    [SerializeField] private GameObject gameMode;
    [SerializeField] private GameObject winLose;
    [SerializeField] private TMP_Text winLoseText;
    void Start()
    {
        ShowGameMode();
    }

    public void HideGameMode()
    {
        gameMode.SetActive(false);
    }
    public void ShowGameMode()
    {
        gameMode.SetActive(true);
        winLose.SetActive(false);
    }

    public void ShowWin()
    {
        HideGameMode();
        winLose.SetActive(true);
        winLoseText.text = "WIN!!!";
    }
    public void ShowLose()
    {
        HideGameMode();
        winLose.SetActive(true);
        winLoseText.text = "LOSE!!!";
    }
}
