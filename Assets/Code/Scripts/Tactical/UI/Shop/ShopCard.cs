using UnityEngine;

public class ShopCard : ShopItem
{
    Card card;
    [SerializeField] private Skill skillData;

    private void Start()
    {
        card = GetComponent<Card>();
    }
    public void RandomCardInput()
    {
        //ī�� �������� ���� �ڵ�
        card.Init(skillData);
       
    }

    public override void BuyAction()
    {
        //�κ��丮�� ��ų �̵�
    }

}
