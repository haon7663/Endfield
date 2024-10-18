using UnityEngine;
using UnityEngine.UI;

public class SkillCastingViewer : MonoBehaviour
{
    public Image fillImage;
    public Image selectSkill;
    public RectTransform rectTransform;

    public Skill Data { get; private set; }

    public void Init(Skill skill, Vector3 pos)
    {
        Data = skill;
        rectTransform.anchoredPosition = pos;
        //selectSkill.sprite = skillSO.skillImage;
        Debug.Log(skill.name);
    }

    private void Update()
    {  
        UpdateSkillCastingTime();       
    }

    private void UpdateSkillCastingTime()
    {
        var fillAmount = Mathf.Clamp01(Data.castingTime / 100f); 
        fillImage.fillAmount = fillAmount;
    }    
}
