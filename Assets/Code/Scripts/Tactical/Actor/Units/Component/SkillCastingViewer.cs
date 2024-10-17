using UnityEngine;
using UnityEngine.UI;

public class SkillCastingViewer : MonoBehaviour
{
    public Image fillImage;
    public Image selectSkill;
    public RectTransform rectTransform;

    private SkillSO skillSO;

    public void Init(SkillSO skillSO, Vector3 Pos)
    {
        this.skillSO = skillSO;
        rectTransform.anchoredPosition = Pos;
        //UpdateSkillImage(skillSO.skillImage);
        Debug.Log(skillSO.name);
    }

    

    private void Update()
    {  
        UpdateSkillCastingTime();       
    }

    private void UpdateSkillCastingTime()
    {
        float fillAmount = Mathf.Clamp01(skillSO.castingTime / 100f); 
        fillImage.fillAmount = fillAmount;
    }
    
    public  void UpdateSkillImage(Sprite skillImage)
    {
        selectSkill.sprite = skillImage;
    }
}
