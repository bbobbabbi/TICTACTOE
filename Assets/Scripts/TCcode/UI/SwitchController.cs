using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;

[RequireComponent(typeof(AudioSource))]
public class SwitchController : MonoBehaviour
{
    [SerializeField] private Image handleImage;
    

    private static readonly Color32 onColor = new Color32(255,255,255,255);
    private static readonly Color32 offColor = new Color32(157,157,157,255);
    private Image _backgroundImage;
    private RectTransform _handleRectTransform;
    private bool _isOn;
    private void Awake()
    {
        _backgroundImage = GetComponent<Image>();    
        _handleRectTransform = handleImage.GetComponent<RectTransform>();
    }
    private void Start()
    {
        _isOn = GameManager.Instance.SetIsSoundToggleOn();
        Seton(_isOn);
    }

    private void Seton(bool isOn) {
        if (isOn)
        {
            _handleRectTransform.DOAnchorPosX(20, 0.2f);
            _backgroundImage.DOBlendableColor(onColor, 0.2f);
        }
        else 
        {
            _handleRectTransform.DOAnchorPosX(-20, 0.2f);
            _backgroundImage.DOBlendableColor(offColor, 0.2f);
        }
        GameManager.Instance.SetIsSoundOffOn(_isOn);
        _isOn = isOn;
    }

    public void OnClickSwitch()
    {
        _isOn = !_isOn;
        Seton(_isOn);
        GameManager.Instance.PlaySound(1);
    }
}
