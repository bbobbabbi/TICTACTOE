using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SocialPlatforms;

public class SigninPanelController : MonoBehaviour
{
    [SerializeField] private TMP_InputField _usernameInputField; 
    [SerializeField] private TMP_InputField _passwordInputField;

    public struct SigninData
    {
        public string username;
        public string password;
    }

    [System.Serializable]
    public struct ScoreResult {
        public string id;
        public string username;
        public string nickname;
        public int score;
    }


    public struct SignInResult {
        public int result;
    }
    public void OnClickSigninButton() {
        var username = _usernameInputField.text;
        var password = _passwordInputField.text;

        if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            return;
        }

        SigninData signinData = new SigninData();
        signinData.username = username;
        signinData.password = password;
        StartCoroutine(NetWorkManager.Instance.Signin(signinData,InitTextField,() => {
            GameManager.Instance.currentUserName = username;
            GameManager.Instance.ChangeToMainScene();
            Destroy(gameObject); }));
    }

    

    private void InitTextField() {
        _usernameInputField.text = string.Empty;
        _passwordInputField.text = string.Empty;
    }
    public void OnClickOppenSignupPannelButton() {
        GameManager.Instance.OpenSignupPanel();
    }
}
