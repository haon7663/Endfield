using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class InventorySkillInfor : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI name;

    [SerializeField] private Image skillImage;

    public void SetInfor(Skill skill)
    {
        name.text = skill.name;
       // skillImage.sprite = skill. 이미지를 받는 변수가 없음
    }
}
