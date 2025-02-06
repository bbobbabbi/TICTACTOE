using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CanvasGroup))]
public class PopupPanelController : Singleton<PopupPanelController>
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TMP_Text contentText;
    [SerializeField] private TMP_Text confirmButtonText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private RectTransform panelRectTransform;
    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        Hide(false);
    }

    public void Show(string content, string confirmButtonText, bool isAnimation,Action confirmAction)
    {
        gameObject.SetActive(true);

        _canvasGroup.alpha = 0f;
        panelRectTransform.localScale = Vector3.zero;

        if (isAnimation)
        {
            panelRectTransform.DOScale(1f,1f);
            _canvasGroup.DOFade(1f, 1f).SetEase(Ease.OutBack);
        }
        else {
            _canvasGroup.alpha = 1;
            panelRectTransform.localScale = Vector3.one;
        }

        contentText.text = content;
        this.confirmButtonText.text = confirmButtonText;
        confirmButton.onClick.AddListener(() =>
        {
            confirmAction();
            Hide(true);
        }
        );
    }

    public void Hide(bool isAnimation) {
        if (isAnimation)
        {
            panelRectTransform.DOScale(0f, 1f).OnComplete(() => {
                contentText.text = "";
                this.confirmButtonText.text = "";
                confirmButton.onClick.RemoveAllListeners();
                gameObject.SetActive(false);
            });
            _canvasGroup.DOFade(0f, 1f).SetEase(Ease.InBack);
        }
        else
        {
            contentText.text = "";
            this.confirmButtonText.text = "";
            confirmButton.onClick.RemoveAllListeners();
            gameObject.SetActive(false);
        }

    }

    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

    }
}
