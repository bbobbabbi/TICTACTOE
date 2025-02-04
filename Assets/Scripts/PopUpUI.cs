using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PopUpUI : MonoBehaviour
{
    public delegate void ButtonClick();
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject text;
    


    public void SetPopUpText(string words) { 
        text.GetComponent<TextMeshProUGUI>().text = words;
    }

    public void SetButtonText(string words)
    {
        button.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = words;
    }

    public void SetButtonEvent(ButtonClick buttonClick)
    {
        button.GetComponent<Button>().onClick.AddListener(() => buttonClick?.Invoke());
    }

 

}
