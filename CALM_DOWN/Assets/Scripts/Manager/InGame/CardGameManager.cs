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

    [Header("Current Card")] 
    [SerializeField] private CardData _currentCard;
    
    [SerializeField] private int _moveStep;
    [SerializeField] private int _checkMoveStep;
    [SerializeField] private List<Button> _prefabs;
    [SerializeField] private List<GameObject> _cardsInit;
    
    public Action OnCardCorrect;
    public Action OnCardNotCorrect;

    [SerializeField] private bool _isPlayGame;
    [SerializeField] private float _playTime;
    void Start()
    {
        _isPlayGame = false;
        _checkMoveStep = 0;
        _cardsInit = new List<GameObject>();
    }

    private void Update()
    {
        if (_isPlayGame)
        {
            _playTime += Time.deltaTime;
        }
    }

    public void EasyMode()
    {
        _gridLayoutGroup.enabled = true;
        _gridColumnSize = 3;
        _gridRowSize = 2;
        _moveStep = 6;
        _prefabs = _cardsPrefab.Take(3).ToList();
        _gridLayoutGroup.cellSize = new Vector2(200, 320);
        InitGame();
    }
    public void NormalMode()
    {
        _gridLayoutGroup.enabled = true;
        _gridColumnSize = 4;
        _gridRowSize = 3;
        _moveStep = 12;
        _prefabs = _cardsPrefab.Take(6).ToList();
        _gridLayoutGroup.cellSize = new Vector2(200, 320);
        InitGame();
    }
    public void HardMode()
    {
        _gridLayoutGroup.enabled = true;
        _gridColumnSize = 5;
        _gridRowSize = 4;
        _moveStep = 20;
        _prefabs = _cardsPrefab.Take(10).ToList();
        _gridLayoutGroup.cellSize = new Vector2(133, 213);
        InitGame();
    }
    private void InitGame()
    {
        _playTime = 0;
        _checkMoveStep = 0;
        _isPlayGame = true;
        _currentCard = new CardData();
        ClearAllCards();
        _gridLayoutGroup.constraintCount = _gridColumnSize;
        GeneralMap(_gridColumnSize, _gridRowSize);
    }
    
    void GeneralMap(int col, int row)
    {
        for (int i = 0; i < (col*row)/2; i++)
        {
            Button temp = _prefabs[i];
            CreateNewCard(temp);
            Button temp2 = _prefabs[i];
            CreateNewCard(temp2);
        }
        ShuffleGridItems();
    }
    void ShuffleGridItems()
    {
        int childCount = _gridLayoutGroup.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = _gridLayoutGroup.transform.GetChild(i);
            int randomIndex = Random.Range(i, childCount);
            Transform randomChild = _gridLayoutGroup.transform.GetChild(randomIndex);
            child.SetSiblingIndex(randomIndex);
            randomChild.SetSiblingIndex(i);
        }
    }
    
    void CreateNewCard(Button cardPrefab)
    {
        GameObject cardButton = Instantiate(cardPrefab, _cardInitTransform).gameObject;
        cardButton.name = cardPrefab.name;
        CardItem item = cardButton.GetComponent<CardItem>();
        CardData cardData = CreateNewCardData(cardButton.name);
        item.SetDataAndOnClickAction(cardData);
        _cardsInit.Add(cardButton);
    }
    
    CardData CreateNewCardData(string name)
    {
        CardData data = new CardData();
        data.SetData(name);
        return data;
    }

    void HideCardByName(string name)
    {
        foreach (GameObject card in _cardsInit)
        {
            if (card.name.ToLower().Contains(name.ToLower()))
            {
                card.SetActive(false);
            }
        }
    }
    public void OnCardClick(CardData cardData)
    {
        _gridLayoutGroup.enabled = false;
        if (string.IsNullOrEmpty(_currentCard.cardName))
        {
            _currentCard.SetData(cardData);
            CardSoundManager.Instance.PlaySFX(CardSoundManager.CardSoundEffectEnum.Click);
        }
        else
        {
            if (_currentCard.cardName.Equals(cardData.cardName))
            {
                HideCardByName(cardData.cardName);
                _currentCard.SetData("");
                CardSoundManager.Instance.PlaySFX(CardSoundManager.CardSoundEffectEnum.Correct);
                OnCardCorrect?.Invoke();
            }
            else
            {
                //_cardsInit[cardData.i,cardData.j].SetActiveButton();
                //_cardsInit[_currentCard.i,_currentCard.j].SetActiveButton();
                _currentCard.SetData("");
                CardSoundManager.Instance.PlaySFX(CardSoundManager.CardSoundEffectEnum.NotCorrect);
                OnCardNotCorrect?.Invoke();
            }

            _checkMoveStep++;
        }

        CheckWin();
        CheckLose();
    }
    
    public void ClearAllCards()
    {
        _cardsInit.Clear();
        foreach (Transform card in _cardInitTransform)
        {
            Destroy(card.gameObject);
        }
    }

    private void CheckWin()
    {
        int cardCorrect = 0;
        foreach (GameObject cardItem in _cardsInit)
        {
            if (!cardItem.gameObject.activeInHierarchy)
                cardCorrect++;
        }

        if (cardCorrect == _cardsInit.Count())// && _checkMoveStep <= _moveStep)
        {
            //Debug.LogError("WIN !!!!");
            _isPlayGame = false;
            CardUIManager.Instance.ShowWin();
            CardSoundManager.Instance.PlaySFX(CardSoundManager.CardSoundEffectEnum.Win);
            CardDataScoreManager.Instance.AddNewTime(_playTime);
            CardDataScoreManager.Instance.SaveFile();
        }
    }

    private void CheckLose()
    {
        if (_checkMoveStep > _moveStep)
        {
            _isPlayGame = false;
            CardUIManager.Instance.ShowLose();
            CardSoundManager.Instance.PlaySFX(CardSoundManager.CardSoundEffectEnum.Lose);
            CardDataScoreManager.Instance.AddNewTime(_playTime);
            CardDataScoreManager.Instance.SaveFile();
        }
    }
}
