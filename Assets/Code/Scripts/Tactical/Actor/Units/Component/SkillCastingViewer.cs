using System.Collections;
using System.Globalization;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillCastingViewer : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public Image selectSkill;
    public TMP_Text castLabel;
    public RectTransform rectTransform;
    public Image backGround;
    public Image fill;

    [SerializeField] private Color targetColor;

    public Skill Data { get; private set; }

    public void Init(Skill skill, Vector3 pos)
    {
        Data = skill;
        //rectTransform.anchoredPosition = pos;
        rectTransform.anchoredPosition = new Vector2(0,90);
        rectTransform.DOAnchorPos(pos, 0.15f).SetEase(Ease.InOutCubic);
        selectSkill.sprite = SkillLoader.GetSkillSprite(skill.spriteName);
        
        castLabel.text = skill.castingTime.ToString("F1");
    }
    
    public IEnumerator Cast()
    {
        var sequence = DOTween.Sequence();
        
        sequence.Append(DOVirtual.Float(1, 0, Data.castingTime, value =>
        {
            fill.fillAmount = 1 - value;
            castLabel.text = (value * Data.castingTime).ToString("F1");
        }).SetEase(Ease.Linear));
        backGround.DOColor(targetColor, Data.castingTime);
        
        yield return sequence.WaitForCompletion();
        
        backGround.rectTransform.SetAsFirstSibling();
        canvasGroup.DOFade(0, 0.15f);
    }
}
