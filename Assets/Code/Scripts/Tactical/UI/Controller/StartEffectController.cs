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
        backGround.rectTransform.DOSizeDelta(new Vector2(backGround.rectTransform.sizeDelta.x, 180), 0.4f)
            .OnComplete(() =>
            {
                startText.DOFade(1, 0.45f);
                underBar.DOFade(1, 0.45f);
                DOVirtual.DelayedCall(1f, () =>
                {
                    underBar.rectTransform.DOMoveX(-1200, 1f).SetEase(Ease.InCubic);
                    startText.rectTransform.DOMoveX(-1200, 1f).SetEase(Ease.InCubic)
                        .OnComplete(() => backGround.DOFade(0, 0.25f));
                });
            });
        
            
    }
}
