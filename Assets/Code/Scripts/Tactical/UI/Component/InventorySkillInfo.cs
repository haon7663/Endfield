using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySkillInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameLabel;
    [SerializeField] private Image icon;

    public void SetInfo(Skill skill)
    {
        nameLabel.text = skill.name;
        //skillImage.sprite = skill. 이미지를 받는 변수가 없음
    }
}
