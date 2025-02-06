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

        //TODO: board의 내용을 보고 다음 수를 계산 후 반환
        var CopyBoard = DeepCopyArray(board);
        //0.보드전체 탐색 플레이어 a의 갯수 ,빈자리 파악
        FindEmptyPositionAndAcount(CopyBoard);
        //1. 플레이어가a 2개 이상이라면 플레이어 a로 뒀을 떄 CheckGameWin을 했을 때 true의 좌표 가져오기
        if (playerACount >= 2) {
            FindPositionWhenSetAThenWin(CopyBoard,PlayerType.PlayerA);
        }
        //2.플레이어b가 2개 이상이라면 플레이어 b로 뒀을 때 CheckGameWin을 했을 때 true의 좌표 가져오기
        else if (playerBCount >=2)
        {
            FindPositionWhenSetAThenWin(CopyBoard,PlayerType.PlayerB);
        }
        //3.위 두 좌표 비교 후 같은 곳이 있으면 그 자리 반환 없다면 보드 전체 탐색후 빈 공간 반환
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
        // 3개 리스트에 모두 존재하는 값 (우선순위 1)
        var commonToAll = list1.Intersect(list2).Intersect(list3).ToList();

        // 2개 리스트에만 존재하는 값 (우선순위 2)
        var commonToTwo = list1.Intersect(list2).Except(commonToAll)
                               .Concat(list1.Intersect(list3).Except(commonToAll))
                               .Concat(list2.Intersect(list3).Except(commonToAll))
                               .Distinct()
                               .ToList();

        // 결과 리스트: 3개 리스트에 공통된 값, 그 후 2개 리스트에만 공통된 값 순으로 결합
        var result = commonToAll.Concat(commonToTwo).ToList();

        // result가 비어있다면 list3의 첫 번째 값을 반환, 아니면 result의 첫 번째 값을 반환
        // 이길 수 있는 패가 있다면 우선
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
                return list3.FirstOrDefault(); // list3에 값이 없으면 기본값을 반환
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
                //빈칸이라면 a를두고
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
