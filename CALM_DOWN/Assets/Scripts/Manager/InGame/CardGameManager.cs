using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardGameManager : Singleton<CardGameManager>
{
    [Header("Map size")] 
    [SerializeField] private int _gridColumnSize;
    [SerializeField] private int _gridRowSize;
    [SerializeField] private GridLayoutGroup _gridLayoutGroup;
    [SerializeField] private Transform cardInitTransform;

    [Header("Prefabs Init")] 
    [SerializeField] private List<Button> cardsPrefab;
    [SerializeField] private List<Button> cardsInit;

    [Header("Current Card Name")] 
    [SerializeField] private string currentCardName;
    void Start()
    {
        cardsInit = new List<Button>();
        _gridLayoutGroup.constraintCount = _gridColumnSize;
        GeneralMapSize(_gridColumnSize, _gridRowSize);
    }

    void GeneralMapSize(int col, int row)
    {
        ClearCardsInit();
        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
            {
                Button temp = GetRandomCard();
                Button card = Instantiate(temp, cardInitTransform);
                card.name = temp.name;
                CardItem item = card.GetComponent<CardItem>();
                item.SetDataAndOnClickAction(card.name,i, j);

            }
        }
    }

    public void OnCardClick(CardData cardData)
    {
        Debug.Log($"{cardData.cardName}");
        if (string.IsNullOrEmpty(currentCardName))
        {
            currentCardName = cardData.cardName;
        }
        else
        {
            if (currentCardName.Equals(cardData.cardName))
            {
                Debug.LogError($"Correct item: {cardData.cardName + cardData.i + cardData.j}");
            }
            else
            {
                Debug.LogError($"Not Correct item: {cardData.cardName + cardData.i + cardData.j}");
            }
            currentCardName = "";   
        }
    }
    Button GetRandomCard()
    {
        int random = Random.Range(0, cardsPrefab.Count);
        return cardsPrefab[random];
    }
    void ClearCardsInit()
    {
        if (cardsInit.Count > 0)
        {
            foreach (var card in cardsInit)
            {
                Destroy(card.gameObject);
            }
        }
        cardsInit.Clear();
    }
}
