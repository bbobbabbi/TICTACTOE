using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private BlockController blockController;

    [SerializeField] private GameObject startPanel;     // �ӽ� ����, ���߿� ���� ����
    [SerializeField] private TMP_Text Text;     // �ӽ� ���� ,���߿� ���� ����
    private enum PlayerType { None, PlayerA, PlayerB }
    private PlayerType[,] _board;

    private enum TurnType { PlayerA, PlayerB }

    private enum GameResult
    {
        None,   // ���� ���� ��
        Win,    // �÷��̾� ��
        Lose,   // �÷��̾� ��
        Draw    // ���
    }

    private void Start()
    {
        // ���� �ʱ�ȭ
        InitGame();
    }

    /// <summary>
    /// ���� �ʱ�ȭ �Լ�
    /// </summary>
    public void InitGame()
    {
        // _board �ʱ�ȭ
        _board = new PlayerType[3, 3];

        // ��� �ʱ�ȭ
        blockController.InitBlocks();
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    public void StartGame()
    {
        startPanel.SetActive(false);        // TODO: �׽�Ʈ �ڵ�, ���߿� ���� ����
        InitGame();
        SetTurn(TurnType.PlayerA);
    }

    /// <summary>
    /// ���� ������ ȣ��Ǵ� �Լ�
    /// gameResult�� ���� ��� ���
    /// </summary>
    /// <param name="gameResult">win, lose, draw</param>
    private void EndGame(GameResult gameResult)
    {
        // TODO: ���߿� ����!!

        switch (gameResult)
        {
            case GameResult.Win:
                Text.text = "Player A �¸�";
                Debug.Log("playerA win");
                startPanel.SetActive(true);
              
                break;
            case GameResult.Lose:
                Text.text = "Player B �¸�";
                Debug.Log("playerB win");
                startPanel.SetActive(true);
                break;
            case GameResult.Draw:
                Text.text = "���";
                Debug.Log("Draw");
                startPanel.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// _board�� ���ο� ���� �Ҵ��ϴ� �Լ�
    /// </summary>
    /// <param name="playerType">�Ҵ��ϰ��� �ϴ� �÷��̾� Ÿ��</param>
    /// <param name="row">Row</param>
    /// <param name="col">Col</param>
    /// <returns>False�� ��ȯ�Ǹ� �Ҵ��� �� ����, True�� �Ҵ��� �Ϸ��</returns>
    private bool SetNewBoardValue(PlayerType playerType, int row, int col)
    {
        if (playerType == PlayerType.PlayerA)
        {
            _board[row, col] = playerType;
            blockController.PlaceMarker(Block.MarkerType.O, row, col);
            return true;
        }
        else if (playerType == PlayerType.PlayerB)
        {
            _board[row, col] = playerType;
            blockController.PlaceMarker(Block.MarkerType.X, row, col);
            return true;
        }
        return false;
    }

    private void SetTurn(TurnType turnType)
    {
        switch (turnType)
        {
            case TurnType.PlayerA:
              
                Debug.Log("Player A turn");
                blockController.onBlockClickedDelegate = (row, col) =>
                {
                    if (SetNewBoardValue(PlayerType.PlayerA, row, col))
                    {
                        var gameResult = CheckGameResult();
                        if (gameResult == GameResult.None)
                            SetTurn(TurnType.PlayerB);
                        else
                            EndGame(gameResult);
                    }
                    else
                    {

                    }
                };

                break;
            case TurnType.PlayerB:
                Debug.Log("Player B turn");
                blockController.onBlockClickedDelegate = (row, col) =>
                {
                    if (SetNewBoardValue(PlayerType.PlayerB, row, col))
                    {
                        var gameResult = CheckGameResult();
                        if (gameResult == GameResult.None)
                            SetTurn(TurnType.PlayerA);
                        else
                            EndGame(gameResult);
                    }
                    else
                    {
                        // TODO: �̹� �ִ� ���� ��ġ���� �� ó��

                    }
                };
                break;
        }
    }

    /// <summary>
    /// ���� ��� Ȯ�� �Լ�
    /// </summary>
    /// <returns>�÷��̾� ���� ���� ���</returns>
    private GameResult CheckGameResult()
    {
        if (CheckGameWin(PlayerType.PlayerA)) { return GameResult.Win; }
        if (CheckGameWin(PlayerType.PlayerB)) { return GameResult.Lose; }
        if (IsAllBlocksPlaced()) { return GameResult.Draw; }

        return GameResult.None;
    }

    /// <summary>
    /// ��� ��Ŀ�� ���忡 ��ġ �Ǿ����� Ȯ���ϴ� �Լ�
    /// </summary>
    /// <returns>True: ��� ��ġ</returns>
    private bool IsAllBlocksPlaced()
    {
        for (var row = 0; row < _board.GetLength(0); row++)
        {
            for (var col = 0; col < _board.GetLength(1); col++)
            {
                if (_board[row, col] == PlayerType.None)
                    return false;
            }
        }
        return true;
    }

    //������ ���и� �Ǵ��ϴ� �Լ�
    private bool CheckGameWin(PlayerType playerType)
    {
        // ���η� ��Ŀ�� ��ġ�ϴ��� Ȯ��
        for (var row = 0; row < _board.GetLength(0); row++)
        {
            if (_board[row, 0] == playerType && _board[row, 1] == playerType && _board[row, 2] == playerType)
            {
                return true;
            }
        }

        // ���η� ��Ŀ�� ��ġ�ϴ��� Ȯ��
        for (var col = 0; col < _board.GetLength(1); col++)
        {
            if (_board[0, col] == playerType && _board[1, col] == playerType && _board[2, col] == playerType)
            {
                return true;
            }
        }

        // �밢�� ��Ŀ ��ġ�ϴ��� Ȯ��
        if (_board[0, 0] == playerType && _board[1, 1] == playerType && _board[2, 2] == playerType)
        {
            return true;
        }
        if (_board[0, 2] == playerType && _board[1, 1] == playerType && _board[2, 0] == playerType)
        {
            return true;
        }

        return false;
    }
}