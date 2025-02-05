using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static GameManager;
using static TurnPanelController;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private BlockController blockController;
    [SerializeField] private PanelManager panelManager;
    public enum PlayerType { None, PlayerA, PlayerB ,Init}

    private PlayerType[,] _board;

    private enum TurnType { PlayerA, PlayerB }

    private enum GameResult
    {
        None,   // 게임 진행 중
        Win,    // 플레이어 승
        Lose,   // 플레이어 패
        Draw    // 비김
    }

    private void SetFieldAgain() {
        if (blockController.IsUnityNull()|| panelManager.IsUnityNull()) {
            blockController = FindAnyObjectByType<BlockController>();
            panelManager = FindAnyObjectByType<PanelManager>();
        }
    }
    private void Start()
    {
        // 게임 초기화
        InitGame();
    }

    /// <summary>
    /// 게임 초기화 함수
    /// </summary>
    public void InitGame()
    {
        SetFieldAgain();
        // _board 초기화
        _board = new PlayerType[3, 3];

        // 블록 초기화
        blockController.InitBlocks();

        StartGame();

    }

    /// <summary>
    /// 게임 시작
    /// </summary>
    public void StartGame()
    {
        panelManager.SetOXPanelEnd(PlayerType.Init,"게임시작");
        panelManager.ShowPanel(PanelManager.PanelType.TurnPanel);
        SetTurn(TurnType.PlayerA);
    }

    /// <summary>
    /// 게임 오버시 호출되는 함수
    /// gameResult에 따라 결과 출력
    /// </summary>
    /// <param name="gameResult">win, lose, draw</param>
    private void EndGame(GameResult gameResult)
    {
        // TODO: 나중에 구현!!
        string text;
        switch (gameResult)
        {
            case GameResult.Win:

                text = "playerA win";
               
                break;
            case GameResult.Lose:
                text = "playerB win";
                break;
            case GameResult.Draw:
                text = "Draw";
                break;
            default:
                text = "";
                break;
        }
        panelManager.SetOXPanelEnd(PlayerType.None,text);
    }

    /// <summary>
    /// _board에 새로운 값을 할당하는 함수
    /// </summary>
    /// <param name="playerType">할당하고자 하는 플레이어 타입</param>
    /// <param name="row">Row</param>
    /// <param name="col">Col</param>
    /// <returns>False가 반환되면 할당할 수 없음, True는 할당이 완료됨</returns>
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
                panelManager.SetOXPanelAlbedoAndTurnText(PlayerType.PlayerA, 1f);
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
                panelManager.SetOXPanelAlbedoAndTurnText(PlayerType.PlayerB, 1f);
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
                        // TODO: 이미 있는 곳을 터치했을 때 처리

                    }
                };
                break;
        }
    }

    /// <summary>
    /// 게임 결과 확인 함수
    /// </summary>
    /// <returns>플레이어 기준 게임 결과</returns>
    private GameResult CheckGameResult()
    {
        if (CheckGameWin(PlayerType.PlayerA)) { return GameResult.Win; }
        if (CheckGameWin(PlayerType.PlayerB)) { return GameResult.Lose; }
        if (IsAllBlocksPlaced()) { return GameResult.Draw; }

        return GameResult.None;
    }

    /// <summary>
    /// 모든 마커가 보드에 배치 되었는지 확인하는 함수
    /// </summary>
    /// <returns>True: 모두 배치</returns>
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

    //게임의 승패를 판단하는 함수
    private bool CheckGameWin(PlayerType playerType)
    {
        // 가로로 마커가 일치하는지 확인
        for (var row = 0; row < _board.GetLength(0); row++)
        {
            if (_board[row, 0] == playerType && _board[row, 1] == playerType && _board[row, 2] == playerType)
            {
                (int, int)[] blocks = { (row, 0), (row, 1), (row, 2) };
                blockController.SetBlockColor(playerType, blocks);
                return true;
            }
        }

        // 세로로 마커가 일치하는지 확인
        for (var col = 0; col < _board.GetLength(1); col++)
        {
            if (_board[0, col] == playerType && _board[1, col] == playerType && _board[2, col] == playerType)
            {

                (int, int)[] blocks = { (0, col), (1, col), (2, col) };
                blockController.SetBlockColor(playerType, blocks);
                return true;
            }
        }

        // 대각선 마커 일치하는지 확인
        if (_board[0, 0] == playerType && _board[1, 1] == playerType && _board[2, 2] == playerType)
        {
            (int, int)[] blocks = { (0, 0), (1, 1), (2, 2) };
            blockController.SetBlockColor(playerType, blocks);
            return true;
        }
        if (_board[0, 2] == playerType && _board[1, 1] == playerType && _board[2, 0] == playerType)
        {
            (int, int)[] blocks = { (0, 2), (1, 1), (2, 0) };
            blockController.SetBlockColor(playerType, blocks);
            return true;
        }

        return false;
    }
}