using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CardGameManager : Singleton<CardGameManager>
{
    [Header("Map size")] 
    [SerializeField] private int _gridColumnSize;
    [SerializeField] private int _gridRowSize;
    [SerializeField] private GridLayoutGroup _gridLayoutGroup;
    [SerializeField] private Transform _cardInitTransform;

    [Header("Prefabs Init")] 
    [SerializeField] private List<Button> _cardsPrefab;
    [SerializeField] private CardItem[,] _cardsInit;
    private Dictionary<string, OddCardData> _countCards;
    private Dictionary<string, OddCardData> _oddCards;
    
    [Header("Current Card")] 
    [SerializeField] private CardData _currentCard;

    [SerializeField] private int _moveStep;
    [SerializeField] private int _checkMoveStep;

    public Action OnCardCorrect;
    public Action OnCardNotCorrect;
    void Start()
    {
        _checkMoveStep = 0;
    }

    public void EasyMode()
    {
        _gridLayoutGroup.enabled = true;
        _gridColumnSize = 3;
        _gridRowSize = 2;
        _moveStep = 3;
        InitGame();
    }
    public void NormalMode()
    {
        _gridLayoutGroup.enabled = true;
        _gridColumnSize = 4;
        _gridRowSize = 3;
        _moveStep = 4;
        InitGame();
    }
    public void HardMode()
    {
        _gridLayoutGroup.enabled = true;
        _gridColumnSize = 5;
        _gridRowSize = 4;
        _moveStep = 5;
        _gridLayoutGroup.cellSize = new Vector2(133, 213);
        InitGame();
    }
    private void InitGame()
    {
        _countCards = new Dictionary<string, OddCardData>();
        _oddCards = new Dictionary<string, OddCardData>();
        _currentCard = new CardData();
        _cardsInit = new CardItem[_gridColumnSize,_gridRowSize];
        _gridLayoutGroup.constraintCount = _gridColumnSize;
        GeneralMapSize(_gridColumnSize, _gridRowSize);
    }
    void GeneralMapSize(int col, int row)
    {
        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
            {
                Button temp = GetRandomCard();
                CreateCard(temp, i, j);
            }
        }
        OddCardsCheck();
    }

    void CountCards(CardData data)
    {
        if (!_countCards.ContainsKey(data.cardName))
        {
            OddCardData oddCardData = new OddCardData
            {
                cardsData = new List<CardData>()
            };
            oddCardData.AddData(data,1);
            _countCards.Add(data.cardName,oddCardData);
        }
        else
        {
            OddCardData countOddCard = _countCards[data.cardName];
            countOddCard.cardCount++;
            countOddCard.AddData(data,countOddCard.cardCount++);
            _countCards[data.cardName] = countOddCard;
        }
    }

    void OddCardsCheck()
    {
        // Get Odd card
        foreach (KeyValuePair<string, OddCardData> keyValuePair in _countCards)
        {
            if (keyValuePair.Value.cardCount % 2 != 0)
            {
                _oddCards.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }

        for (int i = 0; i < _oddCards.Count-1; i+=2)
        {
            for (int j = 1; j < _oddCards.Count; j+=2)
            {
                KeyValuePair<string, OddCardData> firstCards = _oddCards.ElementAt(i);
                KeyValuePair<string, OddCardData> secondCards = _oddCards.ElementAt(j);
                
                int indexRandom = Random.Range(0, firstCards.Value.cardsData.Count);
                CardData randomCard = firstCards.Value.cardsData[indexRandom];
                
                int indexRandom2 = Random.Range(0, secondCards.Value.cardsData.Count);
                CardData randomCard2 = secondCards.Value.cardsData[indexRandom2];
                
                Button temp = GetRandomCardByName(randomCard2.cardName);
                Debug.Log($"Replace old card:{randomCard.cardName} Card: {randomCard.i}{randomCard.j} by Create new odd card name: {randomCard2.cardName}");
                Destroy(_cardsInit[randomCard.i, randomCard.j].gameObject);
                CreateCard(temp, randomCard.i, randomCard.j);
            }
        }
    }

    void CreateCard(Button cardPrefab, int i, int j)
    {
        Button cardButton = Instantiate(cardPrefab, _cardInitTransform);
        cardButton.name = cardPrefab.name;
        CardItem item = cardButton.GetComponent<CardItem>();
        CardData cardData = CreateNewCardData(cardButton.name, i, j);
        item.SetDataAndOnClickAction(cardData);
        CountCards(cardData);
        _cardsInit[i,j] = item;
    }
    CardData CreateNewCardData(string name, int i, int j)
    {
        CardData data = new CardData();
        data.SetData(name,i,j);
        return data;
    }
    public void OnCardClick(CardData cardData)
    {
        _gridLayoutGroup.enabled = false;
        Debug.Log($"On click card: {cardData.cardName + cardData.i + cardData.j}");
        if (string.IsNullOrEmpty(_currentCard.cardName))
        {
            _currentCard.SetData(cardData);
            CardSoundManager.Instance.PlaySFX(CardSoundManager.CardSoundEffectEnum.Click);
        }
        else
        {
            if (_currentCard.cardName.Equals(cardData.cardName))
            {
                Debug.Log($"Correct item: {cardData.cardName + cardData.i + cardData.j}");
                _cardsInit[cardData.i,cardData.j].gameObject.SetActive(false);
                _cardsInit[_currentCard.i,_currentCard.j].gameObject.SetActive(false);
                _currentCard.SetData("",-1,-1);
                CardSoundManager.Instance.PlaySFX(CardSoundManager.CardSoundEffectEnum.Correct);
                OnCardCorrect?.Invoke();
            }
            else
            {
                Debug.Log($"Not Correct item: {cardData.cardName + cardData.i + cardData.j}");
                //_cardsInit[cardData.i,cardData.j].SetActiveButton();
                //_cardsInit[_currentCard.i,_currentCard.j].SetActiveButton();
                _currentCard.SetData("",-1,-1);
                CardSoundManager.Instance.PlaySFX(CardSoundManager.CardSoundEffectEnum.NotCorrect);
                OnCardNotCorrect?.Invoke();
            }

            _checkMoveStep++;
        }

        CheckWin();
        CheckLose();
    }
    
    Button GetRandomCard()
    {
        int random = Random.Range(0, _cardsPrefab.Count);
        return _cardsPrefab[random];
    }
    Button GetRandomCardByName(string cardName)
    {
        foreach (Button card in _cardsPrefab)
        {
            if (card.name.Equals(cardName))
                return card;
        }
        return null;
    }

    public void ClearAllCards()
    {
        foreach (var cardItem in _cardsInit)    
        {
            Destroy(cardItem.gameObject);
        }

        _cardsInit = null;
    }

    private void CheckWin()
    {
        int cardCorrect = 0;
        foreach (CardItem cardItem in _cardsInit)
        {
            if (!cardItem.gameObject.activeInHierarchy)
                cardCorrect++;
        }

        if (cardCorrect == _cardsInit.Length && _checkMoveStep <= _moveStep)
        {
            Debug.LogError("WIN !!!!");
            CardUIManager.Instance.ShowWin();
            CardSoundManager.Instance.PlaySFX(CardSoundManager.CardSoundEffectEnum.Win);
        }
    }

    private void CheckLose()
    {
        if (_checkMoveStep > _moveStep)
        {
            CardUIManager.Instance.ShowLose();
            CardSoundManager.Instance.PlaySFX(CardSoundManager.CardSoundEffectEnum.Lose);
        }
    }
}

public class OddCardData
{
    public List<CardData> cardsData;
    public int cardCount;
    
    public void AddData(CardData card, int count)
    {
        cardsData.Add(card);
        cardCount = count;
    }
}