using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fade : Singleton<Fade>
{
    private Image _fadeImage;

    private void Awake()
    {
        _fadeImage = GetComponent<Image>();
        FadeIn();
    }
    
    private void FadeIn()
    {
        DOTween.Kill(this);
        
        _fadeImage.enabled = true;
        
        var sequence = DOTween.Sequence();
        sequence.AppendInterval(0.2f);
        sequence.Append(_fadeImage.DOFade(0, 0.25f).SetUpdate(true).From(1).OnComplete(() => _fadeImage.enabled = false));
    }
    
    public void FadeOut(Scene scene)
    {
        DOTween.Kill(this);
        
        _fadeImage.enabled = true;
        
        var sequence = DOTween.Sequence();
        sequence.Append(_fadeImage.DOFade(1, 0.5f).SetUpdate(true).From(0));
        sequence.AppendCallback(() =>
        {
            DOTween.KillAll();
            SceneManager.LoadSceneAsync(scene.buildIndex);
        });
    }
}
