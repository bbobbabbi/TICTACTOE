using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ScaleBounce : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;

    private void Start()
    {
        rectTransform.DOScale(1.2f, 1f).SetLoops(-1,LoopType.Yoyo);
    }
}
