using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class CardItem : MonoBehaviour
{
    [SerializeField] Button cardButton;
    [SerializeField] private CardData _data;
    [SerializeField] private float _flipSpeed = 10f; 
    [SerializeField] private bool _isFlipped = false;
    [SerializeField] private bool _isFlippedNotCorrect = false;
    void Start()
    {
        
    }
    private void OnDestroy()
    {
        CardGameManager.Instance.OnCardCorrect -= OnCardCorrect;
        CardGameManager.Instance.OnCardNotCorrect -= OnCardNotCorrect;
    }
    private void Update()
    {
        if (_isFlipped)
        {
            StartCoroutine(FlipCard(() =>
            {
                Debug.Log("Flip animation complete!");
                CardGameManager.Instance.OnCardClick(_data);
            }));
            _isFlipped = false;
        }

        if (_isFlippedNotCorrect)
        {
            StartCoroutine(FlipCard(() =>
            {
                Debug.Log("Card Not Correct - Flip animation complete!");
                cardButton.interactable = true;
            }));
            _isFlippedNotCorrect = false;
        }
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
        Debug.Log($"On Click item name: {gameObject.name + _data.i + _data.j}");
        cardButton.interactable = false;
        _isFlipped = true;
        //CardGameManager.Instance.OnCardClick(_data);
    }
    private void OnCardCorrect()
    {
        Debug.Log("OnCardCorrect");
    }
    private void OnCardNotCorrect()
    {
        Debug.Log("OnCardNotCorrect");
        _isFlippedNotCorrect = true;
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
    public int i;
    public int j;

    public void SetData(string nam, int iInput, int jInput)
    {
        cardName = nam;
        i = iInput;
        j = jInput;
    }
    public void SetData(CardData a)
    {
        cardName = a.cardName;
        i = a.i;
        j = a.j;

    }
}
