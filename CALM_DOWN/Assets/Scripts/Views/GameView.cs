using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameView : BaseView
{
    public GameObject gameMode;
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
    }
}
