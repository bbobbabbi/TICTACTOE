using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnPanelController : PanelController
{
    public enum GameUIMode
    {
        Init,
        TurnA,
        TurnB,
        GameOver
    }

    [SerializeField] private Image oImage;
    [SerializeField] private Image xImage;
    [SerializeField] private TMP_Text TurnText;
    [SerializeField] private Button gameOverButton;


    public void SetImageAlbedo(GameUIMode turn, float albedo)
    {
        switch (turn)
        {
            case GameUIMode.TurnA:
                SetAlbedo(oImage, albedo);
                SetAlbedo(xImage, albedo - 0.5f);
                break;
            case GameUIMode.TurnB:
                SetAlbedo(xImage, albedo);
                SetAlbedo(oImage, albedo - 0.5f);
                break;
        }
    }

    public void SetTurnText(GameUIMode turn)
    {
        switch (turn)
        {
            case GameUIMode.TurnA:
                TurnText.text = "A의 턴 입니다";
                break;
            case GameUIMode.TurnB:
                if(GameManager.Instance.currentGameType == GameManager.GameType.SinglePlayer)
                    TurnText.text = "AI가 고민중입니다...";
                else
                    TurnText.text = "B의 턴 입니다";
                break;
        }
    }

    public void SetGameOverButton(GameUIMode OVER, string text)
    {
        switch (OVER)
        {
            case GameUIMode.GameOver:

                oImage.gameObject.SetActive(false);
                xImage.gameObject.SetActive(false);
                TurnText.text = text;
                SetStreatch(0,0,1,1);
                gameOverButton.gameObject.SetActive(true);
                break;

            case GameUIMode.Init:

                oImage.gameObject.SetActive(true);
                xImage.gameObject.SetActive(true);
                TurnText.gameObject.SetActive(true);
                TurnText.text = text;
                SetStreatch(0, 1, 1, 1);
                gameOverButton.gameObject.SetActive(false);
                break;
        }
    }
  

    private void SetAlbedo(Image image, float albedo)
    {
        var color = image.color;
        image.color = new Color(color.r, color.g, color.b, albedo);
    }

    public void OnClickBackButton()
    {
        GameManager.Instance.OpenConfirmPanel("게임을 종료하시겠습니까?", () => { GameManager.Instance.ChangeToMainScene(); });
    }
    public void OnClickSettingButton()
    {
        GameManager.Instance.OpenSettingPanel();
    }
}
