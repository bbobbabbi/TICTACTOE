using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnPanelController : PanelController
{
    [SerializeField] private Image oImage;
    [SerializeField] private Image xImage;

    public void SetImageAlbedo(GameManager.PlayerType playerType,float albedo ) {
        switch (playerType) {
            case GameManager.PlayerType.PlayerA:
                SetAlbedo(oImage, albedo);
                SetAlbedo(xImage, albedo-0.5f);
                break; 
            case GameManager.PlayerType.PlayerB:
                SetAlbedo(xImage, albedo);
                SetAlbedo(oImage, albedo - 0.5f);
                break;
        }
    }

    private void SetAlbedo(Image image,float albedo)
    {
        var color = image.color;
        image.color = new Color(color.r, color.g, color.b, albedo);
    }
}
