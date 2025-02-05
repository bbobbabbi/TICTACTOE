using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [SerializeField] private PanelController ConfirmPanelController;
    [SerializeField] private PanelController SettingPanelController;
    [SerializeField] private PanelController turnPanelController;

    public enum PanelType { ConfirmPanel, ClosePanel,TurnPanel};

    private PanelController _currentPanelController;

    /// <summary>
    /// ǥ���� �г� ���� �����ϴ� �Լ�
    /// </summary>
    /// <param name="panelType">ǥ���� �г�</param>
    public void ShowPanel(PanelType panelType) 
    {
        switch (panelType) {
            case PanelType.ConfirmPanel:
                ShowPanerlController(ConfirmPanelController);
                break;
            case PanelType.ClosePanel:
                ShowPanerlController(SettingPanelController);
                break; 
            case PanelType.TurnPanel:
                ShowPanerlController(turnPanelController);
                break;

        }
    }

    public void SetOXPanelAlbedoAndTurnText(GameManager.PlayerType playerType, float albedo) {
            
            if (turnPanelController is TurnPanelController currentTurnPanelController) {
                switch (playerType) {
                    case GameManager.PlayerType.PlayerA:
                    currentTurnPanelController.SetImageAlbedo(TurnPanelController.GameUIMode.TurnA, albedo);
                    currentTurnPanelController.SetTurnText(TurnPanelController.GameUIMode.TurnA);
                    break;

                    case GameManager.PlayerType.PlayerB:
                    currentTurnPanelController.SetImageAlbedo(TurnPanelController.GameUIMode.TurnB, albedo);
                    currentTurnPanelController.SetTurnText(TurnPanelController.GameUIMode.TurnB);
                    break;
            }
        }
    }
    public void SetOXPanelEnd(GameManager.PlayerType playerType,string text)
    {

        if (turnPanelController is TurnPanelController currentTurnPanelController)
        {
            switch (playerType)
            {
                case GameManager.PlayerType.None:
                    currentTurnPanelController.SetGameOverButton(TurnPanelController.GameUIMode.GameOver, text);
                break;
                case GameManager.PlayerType.Init:
                    currentTurnPanelController.SetGameOverButton(TurnPanelController.GameUIMode.Init, text);
                    break;
            }
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
