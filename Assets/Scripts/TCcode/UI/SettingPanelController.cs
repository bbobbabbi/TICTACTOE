using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanelController : PanelController
{
    /// <summary>
    /// SFX On/OFF시 호출
    /// </summary>
    /// <param name="value">On/Off값</param>
    public void OnSFXToggleValueChanged(bool value) { 
    
    }

    /// <summary>
    /// BGM On/OFF시 호출
    /// </summary>
    /// <param name="value">On/Off값</param>
    public void OnBGMToggleValueChanged(bool value) { 
    
    }
    /// <summary>
    /// x 버튼 클릭시 호출되는 함수
    /// </summary>
    public void OnClickCloseButton()
    {
        Hide();
    }
}
