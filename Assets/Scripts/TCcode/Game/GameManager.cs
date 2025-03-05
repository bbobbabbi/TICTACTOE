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
        None,   // ���� ���� ��
        Win,    // �÷��̾� ��
        Lose,   // �÷��̾� ��
        Draw    // ���
    }

    /// <summary>
    /// ���ε��� �� �̸��� Game �̶�� InitGame ȣ��
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
    /// ���� �ʱ�ȭ �Լ�
    /// </summary>
    public void InitGame()
    {
        SetFieldAgain();
        // _board �ʱ�ȭ
        _board = new PlayerType[3, 3];

        // blockController�� ���� �� �ʱ�ȭ �� ���� ����
        if (!blockController.IsUnityNull()) { 
            blockController.InitBlocks();
            StartGame();
        }
    }


    /// <summary>
    /// GameManager�� �ʿ��� �ʵ� ���� �ִ��� Ȯ�� �� ���ٸ� ã�Ƽ� �Ҵ�
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
    /// ���� ����
    /// </summary>
    public void StartGame()
    {
        SetOXPanelEnd(PlayerType.Init,"���ӽ���");
        turnPanelController.Show();
        SetTurn(TurnType.PlayerA);
    }

    /// <summary>
    /// ���� ������ ȣ��Ǵ� �Լ�
    /// gameResult�� ���� ��� ���
    /// </summary>
    /// <param name="gameResult">win, lose, draw</param>
    private void EndGame(GameResult gameResult)
    {
        IsPlayerTurn = false;
        // TODO: ���߿� ����!!
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

                    //TODO: ���� row,col��
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
                            // TODO: �̹� �ִ� ���� ��ġ���� �� ó��

                        }
                    };
                    //Block���� Ŭ���� �߻��Ǵ� Invoke�� ��������Ʈ�� �Ҵ� �� �ٷ� Invoke
                    //�ٷ� �θ� ��� ���ڴϱ� ���� ��ٷȴ� �α�
                    StartCoroutine(DelayAction(UnityEngine.Random.Range(0.5f, 3f), result.row, result.col));
                }
                else{
                    IsPlayerTurn = true;
                    Debug.Log("Player B turn");
                    SetOXPanelAlbedoAndTurnText(PlayerType.PlayerB, 1f);
                    //TODO: ���� row,col��
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
                            // TODO: �̹� �ִ� ���� ��ġ���� �� ó��

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
    public bool CheckGameWin(PlayerType playerType)
    {
        // ���η� ��Ŀ�� ��ġ�ϴ��� Ȯ��
        for (var row = 0; row < _board.GetLength(0); row++)
        {
            if (_board[row, 0] == playerType && _board[row, 1] == playerType && _board[row, 2] == playerType)
            {
                (int, int)[] blocks = { (row, 0), (row, 1), (row, 2) };
                blockController.SetBlockColor(playerType, blocks);
                return true;
            }
        }

        // ���η� ��Ŀ�� ��ġ�ϴ��� Ȯ��
        for (var col = 0; col < _board.GetLength(1); col++)
        {
            if (_board[0, col] == playerType && _board[1, col] == playerType && _board[2, col] == playerType)
            {

                (int, int)[] blocks = { (0, col), (1, col), (2, col) };
                blockController.SetBlockColor(playerType, blocks);
                return true;
            }
        }

        // �밢�� ��Ŀ ��ġ�ϴ��� Ȯ��
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
        // ���η� ��Ŀ�� ��ġ�ϴ��� Ȯ��
        for (var row = 0; row < _board.GetLength(0); row++)
        {
            if (_board[row, 0] == playerType && _board[row, 1] == playerType && _board[row, 2] == playerType)
            {
                (int, int)[] blocks = { (row, 0), (row, 1), (row, 2) };
                return true;
            }
        }

        // ���η� ��Ŀ�� ��ġ�ϴ��� Ȯ��
        for (var col = 0; col < _board.GetLength(1); col++)
        {
            if (_board[0, col] == playerType && _board[1, col] == playerType && _board[2, col] == playerType)
            {

                (int, int)[] blocks = { (0, col), (1, col), (2, col) };
                return true;
            }
        }

        // �밢�� ��Ŀ ��ġ�ϴ��� Ȯ��
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
    /// SettingPanel ����
    /// </summary>
    public void OpenSettingPanel() {
        if (!canvasTransform.IsUnityNull()) { 
            var settingPanelObject = Instantiate(settingsPanel,canvasTransform);
            settingPanelObject.GetComponent<PanelController>().Show();
        }
    }

    /// <summary>
    /// ConfirmPanel ����
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