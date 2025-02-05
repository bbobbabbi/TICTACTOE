using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [SerializeField] private PanelController startPanelController;

    public enum PanelType { StartPanel, WinPanel, DrawPanel, LosePanel};

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
            case PanelType.WinPanel:
                break;
            case PanelType.DrawPanel:
                break;
            case PanelType.LosePanel:
                break;

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
            panerlController.Show();
            _currentPanelController = panerlController;
    }
}
