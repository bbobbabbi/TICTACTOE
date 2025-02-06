using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static ConfirmPanelController;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
public class PanelController : MonoBehaviour
{ 
    private RectTransform _rectTransform;
    [SerializeField] private RectTransform panelRectTransform;
    private CanvasGroup _backgroundCanvasGroup;

    private void Awake()
    {
        _backgroundCanvasGroup = GetComponent<CanvasGroup>();
        _rectTransform = GetComponent<RectTransform>();
    }
    /// <summary>
    /// Panel 표시 함수
    /// </summary>
    public void Show() {
        _backgroundCanvasGroup.alpha = 0;
        panelRectTransform.localScale = Vector3.zero;
        _backgroundCanvasGroup.DOFade(1, 0.4f).SetEase(Ease.Linear);
        panelRectTransform.DOScale(1, 0.4f).SetEase(Ease.OutBack);
    }

    /// <summary>
    /// Panel 숨기기 함수
    /// </summary>
    public void Hide()
    {
        _backgroundCanvasGroup.DOFade(0, 0.2f).SetEase(Ease.Linear);
        panelRectTransform.DOScale(0, 0.2f).SetEase(Ease.OutBack).OnComplete(() => Destroy(gameObject));

     }
    public void Hide(OnConfirmButtonClick onConfirmButtonClick)
    {
        onConfirmButtonClick?.Invoke();
    }
    public void SetStreatch(int minRL ,int minUD, int maxRL, int maxUD) {
        _rectTransform.anchorMin = new Vector2(minRL, minUD);
        _rectTransform.anchorMax = new Vector2(maxRL, maxUD);
    }
}
