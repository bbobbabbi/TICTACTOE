using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameManager : MonoBehaviour
{
    public void Open() {
        PopupPanelController.Instance.Show("Hello", "ok",true,() => { Debug.Log("ok≈¨∏Ø");});
    }
}
