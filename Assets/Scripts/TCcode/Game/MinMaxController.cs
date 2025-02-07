using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static GameManager;

public static class MinMaxController 
{

    public static (int row, int col) GetBestMove(GameManager.PlayerType[,] board)
    {
        float BestScore = -1000;
        (int row, int col) returnValue = (0,0);
        for (var row = 0; row < board.GetLength(0); row++)
        {
            for (var col = 0; col < board.GetLength(1); col++)
            {
                if (board[row, col] == GameManager.PlayerType.None)
                {
                    board[row, col] = GameManager.PlayerType.PlayerB;
                    var score = DoMinimax(board, 0, false);
                    if (score > BestScore)
                    {
                        BestScore = score;
                        returnValue = (row, col);
                    }
                    board[row, col] = GameManager.PlayerType.None;
                }
            }
        }

        return returnValue;
    }

    private static float DoMinimax(GameManager.PlayerType[,] board, int depth, bool isMaximizing)
    {
        if (CheckGameWin(GameManager.PlayerType.PlayerA, board))
            return -10 + depth;
        if (CheckGameWin(GameManager.PlayerType.PlayerB, board))
            return 10 - depth;
        if (IsAllBlocksPlaced(board))
            return 0;

        if (isMaximizing)
        {
            var bestScore = float.MinValue;
            for (var row = 0; row < board.GetLength(0); row++)
            {
                for (var col = 0; col < board.GetLength(1); col++)
                {
                    if (board[row, col] == GameManager.PlayerType.None)
                    {
                        board[row, col] = GameManager.PlayerType.PlayerB;
                        var score = DoMinimax(board, depth + 1, false);
                        board[row, col] = GameManager.PlayerType.None;
                        bestScore = Mathf.Max(bestScore, score);
                    }
                }
            }
            return bestScore;
        }
        else
        {
            var bestScore = float.MaxValue;
            for (var row = 0; row < board.GetLength(0); row++)
            {
                for (var col = 0; col < board.GetLength(1); col++)
                {
                    if (board[row, col] == GameManager.PlayerType.None)
                    {
                        board[row, col] = GameManager.PlayerType.PlayerA;
                        var score =  DoMinimax(board, depth + 1, true);
                        board[row, col] = GameManager.PlayerType.None;
                        bestScore = Mathf.Min(bestScore, score);
                    }
                }
            }
            return bestScore;
        }
    }

    /// <summary>
    /// ��� ��Ŀ�� ���忡 ��ġ �Ǿ����� Ȯ���ϴ� �Լ�
    /// </summary>
    /// <returns>True: ��� ��ġ</returns>
    public static bool IsAllBlocksPlaced(GameManager.PlayerType[,] board)
    {
        for (var row = 0; row < board.GetLength(0); row++)
        {
            for (var col = 0; col < board.GetLength(1); col++)
            {
                if (board[row, col] == GameManager.PlayerType.None)
                    return false;
            }
        }
        return true;
    }

    /// <summary>
    /// ������ ���и� �Ǵ��ϴ� �Լ�
    /// </summary>
    /// <param name="playerType"></param>
    /// <param name="board"></param>
    /// <returns></returns>
    private static bool CheckGameWin(GameManager.PlayerType playerType, GameManager.PlayerType[,] board)
    {
        // ���η� ��Ŀ�� ��ġ�ϴ��� Ȯ��
        for (var row = 0; row < board.GetLength(0); row++)
            if (board[row, 0] == playerType && board[row, 1] == playerType && board[row, 2] == playerType)
                return true;

        // ���η� ��Ŀ�� ��ġ�ϴ��� Ȯ��
        for (var col = 0; col < board.GetLength(1); col++)
            if (board[0, col] == playerType && board[1, col] == playerType && board[2, col] == playerType)
                return true;

        // �밢�� ��Ŀ ��ġ�ϴ��� Ȯ��
        if (board[0, 0] == playerType && board[1, 1] == playerType && board[2, 2] == playerType)
            return true;
        if (board[0, 2] == playerType && board[1, 1] == playerType && board[2, 0] == playerType)
            return true;

        return false;
    }
}
