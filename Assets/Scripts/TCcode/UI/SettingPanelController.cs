using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanelController : PanelController
{
    /// <summary>
    /// SFX On/OFF�� ȣ��
    /// </summary>
    /// <param name="value">On/Off��</param>
    public void OnSFXToggleValueChanged(bool value) { 
    
    }

    /// <summary>
    /// BGM On/OFF�� ȣ��
    /// </summary>
    /// <param name="value">On/Off��</param>
    public void OnBGMToggleValueChanged(bool value) { 
    
    }
    /// <summary>
    /// x ��ư Ŭ���� ȣ��Ǵ� �Լ�
    /// </summary>
    public void OnClickCloseButton()
    {
        Hide();
    }
}
