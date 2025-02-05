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
    /// 표시할 패널 정보 전달하는 함수
    /// </summary>
    /// <param name="panelType">표시할 패널</param>
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
