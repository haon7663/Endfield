using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    [SerializeField] private List<Card> cards = new List<Card>();

    private void CardInfo()
    {
        foreach (Card card in cards)
        {
            //카드 랜덤 넣기
        }
    }
}
