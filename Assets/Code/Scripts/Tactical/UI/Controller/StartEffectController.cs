using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartEffectController : MonoBehaviour
{
    [SerializeField] private Image backGround,underBar;
    [SerializeField] private TextMeshProUGUI startText;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        backGround.rectTransform.DOSizeDelta(new Vector2(backGround.rectTransform.sizeDelta.x, 180), 0.8f)
            .OnComplete(() =>
            {
                startText.DOFade(1, 0.9f);
                underBar.DOFade(1, 0.9f);
                DOVirtual.DelayedCall(1.7f, () =>
                {
                    underBar.rectTransform.DOMoveX(-1200, 1.5f).SetEase(Ease.InCubic);
                    startText.rectTransform.DOMoveX(-1200, 1.5f).SetEase(Ease.InCubic)
                        .OnComplete(() => backGround.DOFade(0, 0.5f));
                });
            });
        
            
    }
}
