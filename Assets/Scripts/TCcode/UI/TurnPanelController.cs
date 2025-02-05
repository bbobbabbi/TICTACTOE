using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnPanelController : PanelController
{
    [SerializeField] private Image oImage;
    [SerializeField] private Image xImage;
    [SerializeField] private TMP_Text TurnText;
    


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

    public void SetTurnText(GameManager.PlayerType playerType)
    {
        switch (playerType)
        {
            case GameManager.PlayerType.PlayerA:
                TurnText.text = "A의 턴 입니다";
                break;
            case GameManager.PlayerType.PlayerB:
                TurnText.text = "B의 턴 입니다";
                break;
        }
    }


    private void SetAlbedo(Image image,float albedo)
    {
        var color = image.color;
        image.color = new Color(color.r, color.g, color.b, albedo);
    }
}
