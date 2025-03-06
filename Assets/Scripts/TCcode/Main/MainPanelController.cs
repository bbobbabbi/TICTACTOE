using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class MainPanelController : MonoBehaviour
{
    [SerializeField] private GameObject _SignInButton;
    [SerializeField] private TMP_Text Scoretext;
    [SerializeField] private GameObject LeaderboardPrefab;
    private int Score;
    private void Awake()
    {
        NetWorkManager.Instance.onLogined = (score) =>
        {
            Score = score;
            Scoretext.text = $"Score : {score.ToString()}";
            _SignInButton.GetComponentInChildren<TMP_Text>().text = "·Î±×¾Æ¿ô";
        };
    }

    private void Start()
    {
        NetWorkManager.Instance.OnClickScoreButton();
    }
    public void OnClickSinglePlayButton()
    {
        GameManager.Instance.ChangeToGameScene(GameManager.GameType.SinglePlayer); 
    }
    public void OnClickDualPlayButton()
    {
        GameManager.Instance.ChangeToGameScene(GameManager.GameType.DualPlayer);
    }
    public void OnClickSettingButton()
    {
        GameManager.Instance.OpenSettingPanel();
    }
    public void OnClickSignInButton()
    {
        PlayerPrefs.SetString("sid", null);
        GameManager.Instance.OpenSigninPanel();
    }
    public void OnClickSCoreInButton()
    {
        NetWorkManager.Instance.OnClickScoreButton();
    }

    public void OnClickAddSCoreInButton()
    {
        NetWorkManager.Instance.AddScore();
    }
    public void OnClickLeaderBoardButton()
    {
        Instantiate(LeaderboardPrefab, transform.parent);
    }

}
