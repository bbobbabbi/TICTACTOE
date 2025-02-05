using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmPanelController : PanelController
{
    /// <summary>
    /// Confirm ��ư Ŭ���� ȣ��Ǵ� �Լ�
    /// </summary>
    public void OnClickConfirmButton() {
        GameManager.Instance.InitGame();
        Hide();
    }

    /// <summary>
    /// x ��ư Ŭ���� ȣ��Ǵ� �Լ�
    /// </summary>
    public void OnClickCloseButton() {
        Hide();
    }
}
