using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private BlockController blockController;

    public enum PlayerType { None, PlayerA, PlayerB }
    public enum TrunType { PlayerA, PlayerB }
    private PlayerType[,] _board;

    private void Start()
    {
        //���� �ʱ�ȭ
        InitGame();
        //test
        blockController.onBlockClickedDelegate = (row, col) => { Debug.Log("Row : " + row + ", Col : " + col); };

    }

    /// <summary>
    /// ���� �ʱ�ȭ
    /// </summary>
    public void InitGame()
    {
        _board = new PlayerType[3, 3];

        blockController.InitBlocks();

    }

    private void SetTrun(TrunType turnType)
    {
        switch (turnType)
        {
            case TrunType.PlayerA:
                break;
            case TrunType.PlayerB:
                break;
        }

    }
}
