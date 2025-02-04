using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private Transform canvasTransform;
    [SerializeField] private GameObject popUpUIPrefab;
     private PopUpUI popUpController;
    private GameObject popUp;
    // Start is called before the first frame update
    void Start()
    {
        popUp = Instantiate(popUpUIPrefab,canvasTransform.position,Quaternion.identity);
        popUp.transform.parent = canvasTransform;
        popUpController = popUp.GetComponent<PopUpUI>();
        popUpController.SetPopUpText("bobaba");
        popUpController.SetButtonText("olala");
        popUpController.SetButtonEvent(() =>
        {
            popUp.SetActive(false);
            return;
        }
        );

    }
}
