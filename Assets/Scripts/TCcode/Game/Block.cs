using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private Sprite oSprite;
    [SerializeField] private Sprite xSprite;
    [SerializeField] private SpriteRenderer makerSpriteRenderer;

    public enum MarkerType { None, O, X }

    public delegate void OnBlockClicked(int index);
    public OnBlockClicked onBlockClicked;
    private int _blockIndex;

    /// <summary>
    /// Block �ʱ�ȭ �Լ�
    /// </summary>
    /// <param name="blockIndex">Block �ε���</param>
    /// <param name="onBlockClicked">Block ��ġ �̺�Ʈ</param>
    public void InitMarker(int blockIndex, OnBlockClicked onBlockClicked) 
    { 
        _blockIndex = blockIndex;
        SetMarker(MarkerType.None);
        this.onBlockClicked = onBlockClicked;
    }

    /// <summary>
    /// � ��Ŀ�� ǥ������ �����ϴ� �Լ�
    /// </summary>
    /// <param name="markerType"></param>
    public void SetMarker(MarkerType markerType)
    {
        switch (markerType)
        {
            case MarkerType.O:
                makerSpriteRenderer.sprite = oSprite;
                break;
            case MarkerType.X:
                makerSpriteRenderer.sprite = xSprite;
                break;
            case MarkerType.None:
                makerSpriteRenderer.sprite = null;
                break;
        }
    }
    private void OnMouseUpAsButton()
    {
        if (makerSpriteRenderer.sprite == null)
        {
            onBlockClicked?.Invoke(_blockIndex);
        }
    }
}
