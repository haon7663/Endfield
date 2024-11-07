using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStartController : Singleton<GameStartController>
{
    [SerializeField] private Image backGround,underBar;
    [SerializeField] private TextMeshProUGUI startText;
    
    public void Show()
    {
        backGround.rectTransform.DOSizeDelta(new Vector2(backGround.rectTransform.sizeDelta.x, 180), 0.4f)
            .OnComplete(() =>
            {
                startText.DOFade(0.75f, 0.45f);
                underBar.DOFade(0.75f, 0.45f);
                DOVirtual.DelayedCall(1f, () =>
                {
                    backGround.rectTransform.DOSizeDelta(new Vector2(backGround.rectTransform.sizeDelta.x, 0), 0.4f);
                    startText.DOFade(0, 0.3f);
                    underBar.DOFade(0, 0.3f);
                });
            });
    }
}
