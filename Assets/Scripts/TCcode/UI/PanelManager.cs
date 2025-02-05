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
    /// ǥ���� �г� ���� �����ϴ� �Լ�
    /// </summary>
    /// <param name="panelType">ǥ���� �г�</param>
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
    /// �г��� ǥ���ϴ� �Լ�
    /// ���� �г��� �ִٸ� Hide() ���ο� �г��� Show()
    /// </summary>
    /// <param name="panerlController">ǥ���� �г�</param>
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
