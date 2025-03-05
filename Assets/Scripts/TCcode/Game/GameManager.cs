using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject confirmPanel;
    [SerializeField] private SoundController soundController;
    [SerializeField] private GameObject signinPanel;
    [SerializeField] private GameObject signupPanel;

    private Transform canvasTransform;

    private BlockController blockController;
    
    private TurnPanelController turnPanelController;
    private enum TurnType { PlayerA, PlayerB }
    
    public enum PlayerType { None, PlayerA, PlayerB ,Init}

    public enum GameType { SinglePlayer,DualPlayer}

    public GameType currentGameType;
    public bool IsPlayerTurn { get; private set; }

    private bool isSoundOn;
    
    private PlayerType[,] _board;

    private void Start()
    {
        Application.targetFrameRate = 60;
    }


    private enum GameResult
    {
        None,   // 게임 진행 중
        Win,    // 플레이어 승
        Lose,   // 플레이어 패
        Draw    // 비김
    }

    /// <summary>
    /// 씬로딩시 씬 이름이 Game 이라면 InitGame 호출
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    /// 


    public void PlaySound(int audioNum) {
        if (!soundController.IsUnityNull())
        {
            soundController.PlayAudioClip(audioNum);
        }
    }
    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        canvasTransform = GameObject.FindObjectOfType<Canvas>().GetComponent<Transform>();
        if (scene.name == "Game")
            GameManager.Instance.InitGame();
    }

    /// <summary>
    /// 게임 초기화 함수
    /// </summary>
    public void InitGame()
    {
        SetFieldAgain();
        // _board 초기화
        _board = new PlayerType[3, 3];

        // blockController가 있을 때 초기화 및 게임 시작
        if (!blockController.IsUnityNull()) { 
            blockController.InitBlocks();
            StartGame();
        }
    }


    /// <summary>
    /// GameManager가 필요한 필드 값이 있는지 확인 후 없다면 찾아서 할당
    /// </summary>
    private void SetFieldAgain()
    {
        if (blockController.IsUnityNull() || turnPanelController.IsUnityNull())
        {
            turnPanelController = FindAnyObjectByType<TurnPanelController>();
            blockController = FindAnyObjectByType<BlockController>();
        }
    }

    /// <summary>
    /// 게임 시작
    /// </summary>
    public void StartGame()
    {
        SetOXPanelEnd(PlayerType.Init,"게임시작");
        turnPanelController.Show();
        SetTurn(TurnType.PlayerA);
    }

    /// <summary>
    /// 게임 오버시 호출되는 함수
    /// gameResult에 따라 결과 출력
    /// </summary>
    /// <param name="gameResult">win, lose, draw</param>
    private void EndGame(GameResult gameResult)
    {
        IsPlayerTurn = false;
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
        SetOXPanelEnd(PlayerType.None,text);
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
                IsPlayerTurn = true;
                SetOXPanelAlbedoAndTurnText(PlayerType.PlayerA, 1f);
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
                if (currentGameType == GameType.SinglePlayer) {
                    IsPlayerTurn = false;
                    SetOXPanelAlbedoAndTurnText(PlayerType.PlayerB, 1f);
                    Debug.Log("Player B turn");

                    //TODO: 계산된 row,col값
                    //(int row, int col) result = AIController.FindNextMove(_board);
                    (int row, int col) result = MinMaxController.GetBestMove(_board);

                    blockController.onBlockClickedDelegate = (row, col) =>
                    {
                        if (SetNewBoardValue(PlayerType.PlayerB, result.row, result.col))
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
                    //Block에서 클릭시 발생되던 Invoke를 델리게이트에 할당 후 바로 Invoke
                    //바로 두면 기분 나쁘니까 조금 기다렸다 두기
                    StartCoroutine(DelayAction(UnityEngine.Random.Range(0.5f, 3f), result.row, result.col));
                }
                else{
                    IsPlayerTurn = true;
                    Debug.Log("Player B turn");
                    SetOXPanelAlbedoAndTurnText(PlayerType.PlayerB, 1f);
                    //TODO: 계산된 row,col값
                    //(int row, int col) result = AIController.FindNextMove(_board);
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
                }
                break;
        }
    }
    IEnumerator DelayAction(float delayTime , int row, int col)
    {
        yield return new WaitForSeconds(delayTime);
        blockController.onBlockClickedDelegate?.Invoke(row, col);
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
    public bool CheckGameWin(PlayerType playerType)
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
    public bool CheckGameWin(PlayerType playerType ,PlayerType[,] _board)
    {
        // 가로로 마커가 일치하는지 확인
        for (var row = 0; row < _board.GetLength(0); row++)
        {
            if (_board[row, 0] == playerType && _board[row, 1] == playerType && _board[row, 2] == playerType)
            {
                (int, int)[] blocks = { (row, 0), (row, 1), (row, 2) };
                return true;
            }
        }

        // 세로로 마커가 일치하는지 확인
        for (var col = 0; col < _board.GetLength(1); col++)
        {
            if (_board[0, col] == playerType && _board[1, col] == playerType && _board[2, col] == playerType)
            {

                (int, int)[] blocks = { (0, col), (1, col), (2, col) };
                return true;
            }
        }

        // 대각선 마커 일치하는지 확인
        if (_board[0, 0] == playerType && _board[1, 1] == playerType && _board[2, 2] == playerType)
        {
            (int, int)[] blocks = { (0, 0), (1, 1), (2, 2) };
            return true;
        }
        if (_board[0, 2] == playerType && _board[1, 1] == playerType && _board[2, 0] == playerType)
        {
            (int, int)[] blocks = { (0, 2), (1, 1), (2, 0) };
            return true;
        }
        return false;
    }

    public void SetOXPanelAlbedoAndTurnText(GameManager.PlayerType playerType, float albedo)
    {

        if (turnPanelController is TurnPanelController currentTurnPanelController)
        {
            switch (playerType)
            {
                case GameManager.PlayerType.PlayerA:
                    currentTurnPanelController.SetImageAlbedo(TurnPanelController.GameUIMode.TurnA, albedo);
                    currentTurnPanelController.SetTurnText(TurnPanelController.GameUIMode.TurnA);
                    break;

                case GameManager.PlayerType.PlayerB:
                    currentTurnPanelController.SetImageAlbedo(TurnPanelController.GameUIMode.TurnB, albedo);
                    currentTurnPanelController.SetTurnText(TurnPanelController.GameUIMode.TurnB);
                    break;
            }
        }
    }
    public void SetOXPanelEnd(GameManager.PlayerType playerType, string text)
    {

        if (turnPanelController is TurnPanelController currentTurnPanelController)
        {
            switch (playerType)
            {
                case GameManager.PlayerType.None:
                    currentTurnPanelController.SetGameOverButton(TurnPanelController.GameUIMode.GameOver, text);
                    break;
                case GameManager.PlayerType.Init:
                    currentTurnPanelController.SetGameOverButton(TurnPanelController.GameUIMode.Init, text);
                    break;
            }
        }
    }
    public void ChangeToGameScene(GameType gameType) {
        switch (gameType) {
            case GameType.SinglePlayer:
                currentGameType = GameType.SinglePlayer;
                SceneManager.LoadScene("Game");
                break;
            case GameType.DualPlayer:
                currentGameType = GameType.DualPlayer;
                SceneManager.LoadScene("Game");
                break;
        }
    }
    public void ChangeToMainScene()
    {
        SceneManager.LoadScene("Main");
    }

    /// <summary>
    /// SettingPanel 열기
    /// </summary>
    public void OpenSettingPanel() {
        if (!canvasTransform.IsUnityNull()) { 
            var settingPanelObject = Instantiate(settingsPanel,canvasTransform);
            settingPanelObject.GetComponent<PanelController>().Show();
        }
    }

    /// <summary>
    /// ConfirmPanel 열기
    /// </summary>
    /// <param name="message"></param>
    /// <param name="onConfirmButtonClick"></param>
    public void OpenConfirmPanel(string message, ConfirmPanelController.OnConfirmButtonClick onConfirmButtonClick) {
        if (!canvasTransform.IsUnityNull()) { 
            var confirmPanelObject = Instantiate(confirmPanel,canvasTransform);
            confirmPanelObject.GetComponent<ConfirmPanelController>().Show(message,onConfirmButtonClick);
        }
    }

    public void OpenSigninPanel() {
        if (!canvasTransform.IsUnityNull())
        {
            var signinPanelObj = Instantiate(signinPanel, canvasTransform);
        }
    }
    public void OpenSignupPanel()
    {
        if (!canvasTransform.IsUnityNull())
        {
            var signupPanelObj = Instantiate(signupPanel, canvasTransform);
        }
    }

    public void SetIsSoundOffOn(bool isOn) {
        if (isOn) {
            soundController.SoundOff();
        }
        else { 
            soundController.SoundOn();
        }
        isSoundOn = isOn;
    }

    public bool SetIsSoundToggleOn()
    {
        return isSoundOn;
    }

}