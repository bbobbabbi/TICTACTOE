using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class PanelController : MonoBehaviour
{
   
    private RectTransform _rectTransform;
    

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
     
    }
    /// <summary>
    /// Panel ǥ�� �Լ�
    /// </summary>
    public void Show() {
    }

    /// <summary>
    /// Panel ����� �Լ�
    /// </summary>
    public void Hide()
    {
     }

    public void SetStreatch(int minRL ,int minUD, int maxRL, int maxUD) {
        _rectTransform.anchorMin = new Vector2(minRL, minUD);
        _rectTransform.anchorMax = new Vector2(maxRL, maxUD);
    }
}
