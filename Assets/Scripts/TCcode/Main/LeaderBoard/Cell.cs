using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] private TMP_Text username;
    [SerializeField] private TMP_Text score;
     public int Index { get; private set; }
    public void SetItem(Item item,int index = 0) { 
        username.text = item.username;
        score.text = item.score;
        Index = index; 
    }

    public void ChangeStyle() {
        GetComponent<Image>().color = Color.white;
        username.color = Color.black;
        score.color = Color.black;
    }

    public void InitCellStyle() {
        GetComponent<Image>().color = new Color32(250,159,167,100);
        username.color = Color.white;
        score.color = Color.white;
    }
}
