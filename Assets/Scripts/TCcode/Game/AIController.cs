using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using static GameManager;
using System;
using Unity.Mathematics;

public static class AIController
{
    static int playerACount;
    static int playerBCount;
    static List<(int row, int col)> nextAWinPositions ;
    static List<(int row, int col)> nextBWinPositions ;
    static List<(int row, int col)> emptyPositions ;


    public static (int row, int col) FindNextMove(GameManager.PlayerType[,] board) {


       nextAWinPositions = new List<(int row, int col)>();
       nextBWinPositions = new List<(int row, int col)>();
       emptyPositions = new List<(int row, int col)>();

        //TODO: board�� ������ ���� ���� ���� ��� �� ��ȯ
        var CopyBoard = DeepCopyArray(board);
        //0.������ü Ž�� �÷��̾� a�� ���� ,���ڸ� �ľ�
        FindEmptyPositionAndAcount(CopyBoard);
        //1. �÷��̾a 2�� �̻��̶�� �÷��̾� a�� ���� �� CheckGameWin�� ���� �� true�� ��ǥ ��������
        if (playerACount >= 2) {
            FindPositionWhenSetAThenWin(CopyBoard,PlayerType.PlayerA);
        }
        //2.�÷��̾�b�� 2�� �̻��̶�� �÷��̾� b�� ���� �� CheckGameWin�� ���� �� true�� ��ǥ ��������
        else if (playerBCount >=2)
        {
            FindPositionWhenSetAThenWin(CopyBoard,PlayerType.PlayerB);
        }
        //3.�� �� ��ǥ �� �� ���� ���� ������ �� �ڸ� ��ȯ ���ٸ� ���� ��ü Ž���� �� ���� ��ȯ
        if (nextAWinPositions.Count > 0 || nextBWinPositions.Count > 0)
        {
            (int, int) result = CompareLists(nextAWinPositions, nextBWinPositions, emptyPositions);
            initAll();
            return (result);

        }
        else {
            System.Random random = new System.Random();
            var result = emptyPositions[random.Next(0,emptyPositions.Count)];
            initAll();
            return result;
        }
    }

    private static void initAll() {
        playerACount= 0;
        playerBCount=0;
        nextAWinPositions = null;
        nextBWinPositions = null;
        emptyPositions = null;
    }
    private static (int,int) CompareLists(List<(int row, int col)> list1, List<(int row, int col)> list2, List<(int row, int col)> list3)
    {
        // 3�� ����Ʈ�� ��� �����ϴ� �� (�켱���� 1)
        var commonToAll = list1.Intersect(list2).Intersect(list3).ToList();

        // 2�� ����Ʈ���� �����ϴ� �� (�켱���� 2)
        var commonToTwo = list1.Intersect(list2).Except(commonToAll)
                               .Concat(list1.Intersect(list3).Except(commonToAll))
                               .Concat(list2.Intersect(list3).Except(commonToAll))
                               .Distinct()
                               .ToList();

        // ��� ����Ʈ: 3�� ����Ʈ�� ����� ��, �� �� 2�� ����Ʈ���� ����� �� ������ ����
        var result = commonToAll.Concat(commonToTwo).ToList();

        // result�� ����ִٸ� list3�� ù ��° ���� ��ȯ, �ƴϸ� result�� ù ��° ���� ��ȯ
        // �̱� �� �ִ� �а� �ִٸ� �켱
        if (list2.Count > 0) {
            return list2.First();
        }
        else {
            if (result.Any())
            {
                return result.First();
            }
            else
            {
                return list3.FirstOrDefault(); // list3�� ���� ������ �⺻���� ��ȯ
            }
        }
    }

    public static PlayerType[,] DeepCopyArray(GameManager.PlayerType[,] board) {
        PlayerType[,] resualtArray = new PlayerType[board.GetLength(0), board.GetLength(1)];

        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                resualtArray[i, j] = board[i, j];
            }
        }
        return resualtArray;
    }
    private static void FindPositionWhenSetAThenWin(GameManager.PlayerType[,] board,PlayerType playerType)
    {
        for (var row = 0; row < board.GetLength(0); row++)
        {
            for (var col = 0; col < board.GetLength(1); col++)
            {
                //��ĭ�̶�� a���ΰ�
                if (board[row, col] == PlayerType.None)
                {
                    board[row, col] = playerType;
                    if (GameManager.Instance.CheckGameWin(playerType,board)) {
                        if (playerType == PlayerType.PlayerA) { 
                            nextAWinPositions.Add((row,col));
                        }
                        else if (playerType == PlayerType.PlayerB)
                        {
                            nextBWinPositions.Add((row, col));
                        }
                    }
                    board[row,col] = PlayerType.None;
                }
            }
        }
    }
    private static void FindEmptyPositionAndAcount(GameManager.PlayerType[,] board)
    {
        for (var row = 0; row < board.GetLength(0); row++)
        {
            for (var col = 0; col < board.GetLength(1); col++)
            {
                if (board[row, col] == PlayerType.None)
                    emptyPositions.Add((row, col));
                else if(board[row, col] == PlayerType.PlayerA)
                    playerACount++; 
                else if(board[row, col] == PlayerType.PlayerB)
                    playerBCount++;
            }
        }
    }
}
