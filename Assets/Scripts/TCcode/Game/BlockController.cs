using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    [SerializeField] private Block[] blocks;

    public delegate void OnBlockClicked(int row, int col);
    public OnBlockClicked onBlockClickedDelegate;

    public void InitBlocks() 
    {
        for (int i = 0; i < blocks.Length; i++) 
        {
            blocks[i].InitMarker(i, blockIndex => {
                var clickedRow = blockIndex / 3;
                var clickedCol = blockIndex % 3;
                onBlockClickedDelegate?.Invoke(clickedRow, clickedCol);
            }
            );
        }
    }

    /// <summary>
    /// Ư�� Block�� ��Ŀ ǥ���ϴ� �Լ�
    /// </summary>
    /// <param name="markerType"></param>
    /// <param name="row"></param>
    /// <param name="col"></param>
    public void PlaceMarker(Block.MarkerType markerType, int row, int col)
    {
        //row,col�� index�� ��ȯ
        var markerIndex = row * 3 + col;

        //Block���� ��Ŀ ǥ��
        blocks[markerIndex].SetMarker(markerType);
    }

}
