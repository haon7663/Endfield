using System.Collections;
using System.Globalization;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillCastingViewer : MonoBehaviour
{
    public Image fillImage;
    public Image selectSkill;
    public TMP_Text castLabel;
    public RectTransform rectTransform;

    public Skill Data { get; private set; }

    public void Init(Skill skill, Vector3 pos)
    {
        Data = skill;
        rectTransform.anchoredPosition = pos;
        //selectSkill.sprite = skillSO.skillImage;
        
        castLabel.text = skill.castingTime.ToString("F1");
    }
    
    public IEnumerator Cast()
    {
        var tween = DOVirtual.Float(1, 0, Data.castingTime, value =>
        {
            fillImage.fillAmount = value;
            castLabel.text = (value * Data.castingTime).ToString("F1");
        }).SetEase(Ease.Linear);
        yield return tween.WaitForCompletion();
    }
}