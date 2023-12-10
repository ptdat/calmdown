using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSoundManager : Singleton<CardSoundManager>
{
    [SerializeField] private AudioSource win;
    [SerializeField] private AudioSource lose;
    [SerializeField] private AudioSource correct;
    [SerializeField] private AudioSource notCorrect;
    [SerializeField] private AudioSource click;
    [SerializeField] private AudioSource timer;
    
    public enum CardSoundEffectEnum
    {
        Win,
        Lose,
        Correct,
        NotCorrect,
        Click,
        Timer
    }

    public void PlaySFX(CardSoundEffectEnum type)
    {
        switch (type)
        {
            case CardSoundEffectEnum.Win:
                win.Stop();
                win.Play();
                break;
            case CardSoundEffectEnum.Lose:
                lose.Stop();
                lose.Play();
                break;
            case CardSoundEffectEnum.Correct:
                correct.Stop();
                correct.Play();
                break;
            case CardSoundEffectEnum.NotCorrect:
                notCorrect.Stop();
                notCorrect.Play();
                break;
            case CardSoundEffectEnum.Click:
                click.Stop();
                click.Play();
                break;
            case CardSoundEffectEnum.Timer:
                timer.Stop();
                timer.Play();
                break;
            default:
                break;
        }
    }
    
}
