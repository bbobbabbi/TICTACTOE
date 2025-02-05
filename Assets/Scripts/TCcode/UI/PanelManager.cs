using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [SerializeField] private PanelController startPanelController;
    [SerializeField] private PanelController ConfirmPanelController;
    [SerializeField] private PanelController SettingPanelController;
    [SerializeField] private PanelController TurnPanelController;

    public enum PanelType { StartPanel, ConfirmPanel, ClosePanel,TurnPanel};

    private PanelController _currentPanelController;

    /// <summary>
    /// 표시할 패널 정보 전달하는 함수
    /// </summary>
    /// <param name="panelType">표시할 패널</param>
    public void ShowPanel(PanelType panelType) 
    {
        switch (panelType) { 
            case PanelType.StartPanel:
                ShowPanerlController(startPanelController);
                break;
            case PanelType.ConfirmPanel:
                ShowPanerlController(ConfirmPanelController);
                break;
            case PanelType.ClosePanel:
                ShowPanerlController(SettingPanelController);
                break; 
            case PanelType.TurnPanel:
                ShowPanerlController(TurnPanelController);
                break;

        }
    }

    public void SetOXPanelAlbedo(GameManager.PlayerType playerType, float albedo) {
            if (TurnPanelController is TurnPanelController trunPanelController) {
                 trunPanelController.SetImageAlbedo(playerType, albedo);
            }
    }

    /// <summary>
    /// 패널을 표시하는 함수
    /// 기존 패널이 있다면 Hide() 새로운 패널을 Show()
    /// </summary>
    /// <param name="panerlController">표시할 패널</param>
    private void ShowPanerlController(PanelController panerlController) {
        if (_currentPanelController != null) {
            _currentPanelController.Hide();
        }
        panerlController.Show(() => { 
            _currentPanelController = null;
        });
            _currentPanelController = panerlController;
    }
}
