using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    [SerializeField] private Transform sellCards;
    private List<ShopCard> _cards = new List<ShopCard>();

    private void Start()
    {
        for (int i = 0; i < sellCards.childCount; i++)
        {
            _cards.Add(sellCards.GetChild(i).GetComponent<ShopCard>());
        }
        CardInfo();
    }

    private void CardInfo()
    {
        foreach (ShopCard card in _cards)
        {
            //카드 랜덤 넣기
            card.RandomCardInput();
           
        }
    }
}
