using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [SerializeField] private PanelController startPanelController;

    public enum PanelType { StartPanel, WinPanel, DrawPanel, LosePanel};
    public void ShowPanel(PanelType panelType) 
    {
        switch (panelType) { 
            case PanelType.StartPanel:
                break;
            case PanelType.WinPanel:
                break;
            case PanelType.DrawPanel:
                break;
            case PanelType.LosePanel:
                break;

        }
    }
}
