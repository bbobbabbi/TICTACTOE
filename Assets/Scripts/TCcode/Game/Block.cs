using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer))]
public class Block : MonoBehaviour
{
    [SerializeField] private Sprite oSprite;
    [SerializeField] private Sprite xSprite;
    [SerializeField] private SpriteRenderer makerSpriteRenderer;

    public enum MarkerType { None, O, X }

    public delegate void OnBlockClicked(int index);
    private OnBlockClicked _onBlockClicked;
    private int _blockIndex;
    private Color _defaultColor;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultColor = _spriteRenderer.color;
    }

    /// <summary>
    /// ���� ������ �����ϴ� �Լ�
    /// </summary>
    /// <param name="color">����</param>
    public void SetColor(Color color) { 
        _spriteRenderer.color = color;
    }


    /// <summary>
    /// Block �ʱ�ȭ �Լ�
    /// </summary>
    /// <param name="blockIndex">Block �ε���</param>
    /// <param name="onBlockClicked">Block ��ġ �̺�Ʈ</param>
    public void InitMarker(int blockIndex, OnBlockClicked onBlockClicked) 
    {
        _blockIndex = blockIndex;
        SetMarker(MarkerType.None);
        _onBlockClicked = onBlockClicked;
        SetColor(_defaultColor);
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
        if (makerSpriteRenderer.sprite == null && !EventSystem.current.IsPointerOverGameObject()&& GameManager.Instance.IsPlayerTurn)
        {
            GameManager.Instance.PlaySound(0);
            _onBlockClicked?.Invoke(_blockIndex);
        }
    }
}
