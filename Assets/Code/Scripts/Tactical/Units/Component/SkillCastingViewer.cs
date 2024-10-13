using UnityEngine;
using UnityEngine.UI;

public class SkillCastingViewer : MonoBehaviour
{
    public Image fillImage;
    
    public SkillSO skillSO;

    private void Update()
    {
        UpdateHealthBarFill();
    }

    private void UpdateHealthBarFill()
    {
        float fillAmount = Mathf.Clamp01(skillSO.castingTime / 100f); 
        fillImage.fillAmount = fillAmount;
    }
}
