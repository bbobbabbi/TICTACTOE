using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmPanelController : PanelController
{
    [SerializeField] private TMP_Text messageText;

    public delegate void OnConfirmButtonClick();
    private OnConfirmButtonClick onConfirmButtonClick;


    public void Show(string message, OnConfirmButtonClick onConfirmButtonClick) {
        messageText.text = message;
        this.onConfirmButtonClick = onConfirmButtonClick;
        base.Show();
    }
    

    /// <summary>
    /// Confirm ��ư Ŭ���� ȣ��Ǵ� �Լ�
    /// </summary>
    public void OnClickConfirmButton() {

        Hide(onConfirmButtonClick);
    }

    /// <summary>
    /// x ��ư Ŭ���� ȣ��Ǵ� �Լ�
    /// </summary>
    public void OnClickCloseButton() {
        base.Hide();
    }
}
