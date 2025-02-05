using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class PanelController : MonoBehaviour
{
    public bool IsShow { get; private set; }
    public delegate void OnHide();
    private OnHide _onHideDelegate;

    private RectTransform _rectTransform;
    private Vector2 _hideAnchorPosition;


    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _hideAnchorPosition = _rectTransform.anchoredPosition;
        IsShow = false;
    }
    /// <summary>
    /// Panel 표시 함수
    /// </summary>
    public void Show(OnHide onHideDeligate) {
        _onHideDelegate = onHideDeligate;
        _rectTransform.anchoredPosition = Vector2.zero;
        IsShow = true;
    }

    /// <summary>
    /// Panel 숨기기 함수
    /// </summary>
    public void Hide()
    {
        _rectTransform.anchoredPosition = _hideAnchorPosition;
        IsShow = false;
        _onHideDelegate?.Invoke();
    }

    public void SetStreatch(int minRL ,int minUD, int maxRL, int maxUD) {
        _rectTransform.anchorMin = new Vector2(minRL, minUD);
        _rectTransform.anchorMax = new Vector2(maxRL, maxUD);
    }
}
