using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverButtonController : MonoBehaviour
{
    public void OnClickButton()
    {
        GameManager.Instance.OpenConfirmPanel("게임을 종료하시겠습니까?", () => { GameManager.Instance.ChangeToMainScene();});
    }
}
