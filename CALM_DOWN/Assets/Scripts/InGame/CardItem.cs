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
    void Start()
    {
        
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
    }
    void OnCardClick()
    {
        Debug.Log($"On Click item name: {gameObject.name + _data.i + _data.j}");
        cardButton.interactable = false;
        CardGameManager.Instance.OnCardClick(_data);
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
