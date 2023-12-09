using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardItem : MonoBehaviour
{
    [SerializeField] Button cardButton;
    [SerializeField] private CardData _data;
    void Start()
    {
        
    }

    public void SetDataAndOnClickAction(string cardNam, int i, int j)
    {
        _data = new CardData();
        _data.SetData(cardNam, i, j);
        cardButton.onClick.RemoveAllListeners();
        cardButton.onClick.AddListener(OnCardClick);
    }
    void OnCardClick()
    {
        Debug.Log($"On Click item name: {gameObject.name}");
        CardGameManager.Instance.OnCardClick(_data);
    }
    
}

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
}
