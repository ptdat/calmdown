using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class CardItem : MonoBehaviour
{
    [SerializeField] Button cardButton;
    [SerializeField] GameObject img;
    [SerializeField] private CardData _data;
    [SerializeField] private float _flipSpeed = 10f; 
    [SerializeField] private bool _isFlipped = false;
    [SerializeField] private bool _isFlippedNotCorrect = false;
    private float time;
    void Start()
    {
        img.SetActive(false);
    }
    private void OnDestroy()
    {
        CardGameManager.Instance.OnCardCorrect -= OnCardCorrect;
        CardGameManager.Instance.OnCardNotCorrect -= OnCardNotCorrect;
    }
    private void Update()
    {
        // if (_isFlipped)
        // {
        //     // Vector3 targetScale = _isFlipped ? new Vector3(-1f, 1f, 1f) : new Vector3(1f, 1f, 1f);
        //     // transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * _flipSpeed);
        //     // time += Time.deltaTime;
        //     // if (time >= .75)
        //     // {
        //     //     img.SetActive(true);
        //     //     if (time >= 2f)
        //     //     {
        //     //         time = 0;
        //     //         _isFlipped = false;
        //     //         CardGameManager.Instance.OnCardClick(_data);
        //     //     }
        //     // }
        //
        //     //  StartCoroutine(FlipCard(() =>
        //     //  {
        //     //      CardGameManager.Instance.OnCardClick(_data);
        //     //  }));
        //     // _isFlipped = false;
        // }
        //
        // if (_isFlippedNotCorrect)
        // {
        //     img.SetActive(false);
        //     StartCoroutine(FlipCard(() =>
        //     {
        //         cardButton.interactable = true;
        //     }));
        //     _isFlippedNotCorrect = false;
        // }
    }
   
    public void SetActiveButton()
    {
        cardButton.interactable = true;
    }
    public void SetDataAndOnClickAction(CardData cardDat)
    {
        _data = new CardData();
        _data.SetData(cardDat);
        cardButton.onClick.RemoveAllListeners();
        cardButton.onClick.AddListener(OnCardClick);
        CardGameManager.Instance.OnCardCorrect += OnCardCorrect;
        CardGameManager.Instance.OnCardNotCorrect += OnCardNotCorrect;
    }
    void OnCardClick()
    {
        _isFlipped = true;
        cardButton.interactable = false;
        transform.DOScale(new Vector3(-1f, 1f, 1f), .5f).OnComplete(() =>
        {
            img.SetActive(true);
            _isFlipped = false;
            DOVirtual.DelayedCall(1f, () =>
            {
                CardGameManager.Instance.OnCardClick(_data);
            });
        });
    }
    private void OnCardCorrect()
    {
        //Debug.Log("OnCardCorrect");
    }
    private void OnCardNotCorrect()
    {
        //Debug.Log("OnCardNotCorrect");
        _isFlippedNotCorrect = true;
        img.SetActive(false);
        transform.DOScale(new Vector3(1f, 1f, 1f), .5f).OnComplete(() =>
        {
            cardButton.interactable = true;
        });
    }
    
    IEnumerator FlipCard(Action onComplete = null)
    {
        Vector3 targetScale = _isFlipped ? new Vector3(-1f, 1f, 1f) : new Vector3(1f, 1f, 1f);
        while (transform.localScale != targetScale)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * _flipSpeed);
            yield return null;
        }
        onComplete?.Invoke();
    }
}
[Serializable]
public class CardData
{
    public string cardName;
    public void SetData(string name)
    {
        cardName = name;
    }
    public void SetData(CardData a)
    {
        cardName = a.cardName;
    }
}
