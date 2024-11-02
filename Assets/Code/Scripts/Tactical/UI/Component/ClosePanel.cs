using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ClosePanel : MonoBehaviour
{
    public Action onClose;

    [SerializeField] private KeyCode keyCode;
    [SerializeField] private Image fillImage;
    [SerializeField] private float fillSpeed = 2f;
    [SerializeField] private float releaseSpeed = 20f;
    
    private bool _canHold = true;
    private float _fillAmount;

    private void Update()
    {
        if (!_canHold) return;
        
        if (Input.GetKey(keyCode))
        {
            _fillAmount += Time.deltaTime * fillSpeed;
        }
        else
        {
            _fillAmount = Mathf.Lerp(_fillAmount, 0, Time.deltaTime * releaseSpeed); 
        }

        _fillAmount = Mathf.Clamp01(_fillAmount);
        fillImage.fillAmount = _fillAmount;
        
        if (_fillAmount < 1) return;
        fillImage.fillAmount = 0;
        _fillAmount = 0;
        onClose?.Invoke();
        _canHold = false;
        DOVirtual.DelayedCall(0.3f,()=> _canHold = true);
    }
}
