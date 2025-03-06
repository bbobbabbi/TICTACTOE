using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using static NetWorkManager;
using static SigninPanelController;


public class ScoreResultList
{
    public ScoreResult[] allUsers;
}

public class NetWorkManager : Singleton<NetWorkManager>
{
    public delegate void Onlogined(int score);
    public Onlogined onLogined;
    private int currentScore;
    public struct ScoreData
    {
        public int score;
    }
    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
    }
    public IEnumerator Signup(SignupData signupData, Action init, Action success)
    {
        string jsonStr = JsonUtility.ToJson(signupData);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonStr);


        // IDisposable 애들은 파일 관련 애들 다 썼으면 명확하게 없애줘야 다른 애들이 접근할 수 있다 www.Dispose();
        // c#에서는 using을 사용해 이를 생략할 수 있다

        using (UnityWebRequest www = new UnityWebRequest($"{Constants.ServerURL}/users/signup", UnityWebRequest.kHttpVerbPOST))
        {
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();
            init?.Invoke();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log($"Error: {www.error}");
                if (www.responseCode == 409)
                {
                    GameManager.Instance.OpenConfirmPanel("이미 존재하는 사용자입니다", () => init?.Invoke());
                    Debug.Log("중복사용자");
                }
            }
            else
            {
                var result = www.downloadHandler.text;
                Debug.Log($"Result: {result}");
                 GameManager.Instance.OpenConfirmPanel("회원가입이 완료되었습니다", () => success?.Invoke());
            }
        }
    }

    public void OnClickScoreButton()
    {
        StartCoroutine(GetScore(() => { GameManager.Instance.OpenSigninPanel(); },
            (userScore) => { GameManager.Instance.currentUserName = userScore.username; onLogined?.Invoke(userScore.score); }));
    }

    IEnumerator GetScore(Action fail, Action<ScoreResult> success) {
        using (UnityWebRequest www = new UnityWebRequest($"{Constants.ServerURL}/users/score", UnityWebRequest.kHttpVerbGET)) {

            www.downloadHandler = new DownloadHandlerBuffer();

            string sid = PlayerPrefs.GetString("sid", "");
            if (!string.IsNullOrEmpty(sid)) { 
                www.SetRequestHeader("Cookie", sid);
            }
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                if (www.responseCode == 403) {
                    Debug.Log("로그인이 필요합니다.");
                }
                fail?.Invoke();
            }
            else {
                var result = www.downloadHandler.text;
                var userScore = JsonUtility.FromJson<ScoreResult>(result);
                currentScore = userScore.score;
                Debug.Log($"Score: {userScore.score}");
                success?.Invoke(userScore);
            }
        }
    }

    public void OnClickAllScoreButton(Action finished = null)
    {
        StartCoroutine(GetAllScore(() => { GameManager.Instance.OpenSigninPanel(); },
            (userScores) =>
                {
                    GameManager.Instance.scoreResult = userScores;
                    finished?.Invoke();
                }
            ));
    }
    IEnumerator GetAllScore(Action fail, Action<ScoreResultList> success)
    {
        using (UnityWebRequest www = new UnityWebRequest($"{Constants.ServerURL}/users/allscore", UnityWebRequest.kHttpVerbGET))
        {

            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                if (www.responseCode == 403)
                {
                    Debug.Log("로그인이 필요합니다.");
                }
                fail?.Invoke();
            }
            else
            {
                var result = www.downloadHandler.text;
                var userScores = JsonUtility.FromJson<ScoreResultList>("{\"allUsers\":" + result + "}");
                success?.Invoke(userScores);
            }
        }
    }

    public void AddScore() {
        ScoreData scoreData = new ScoreData();
        scoreData.score = currentScore + 10;
        StartCoroutine(SetScore(scoreData, () => { }, () => { }));
    }
    IEnumerator SetScore(ScoreData score, Action fail, Action success)
    {
        string jsonStr = JsonUtility.ToJson(score);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonStr);

        using (UnityWebRequest www = new UnityWebRequest($"{Constants.ServerURL}/users/addscore", UnityWebRequest.kHttpVerbPOST))
        {
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();
      
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                if (www.responseCode == 404)
                {
                    Debug.Log("사용자를 찾을 수 없습니다.");
                }
                if (www.responseCode == 403)
                {
                    Debug.Log("로그인이 필요합니다.");
                }
                fail?.Invoke();
            }
            else
            {
                var result = www.downloadHandler.text;
                Debug.Log($"Result: {result}");
                success?.Invoke();
            }
        }
    }

    public IEnumerator Signin(SigninData signinData, Action init, Action success)
    {
        string jsonString = JsonUtility.ToJson(signinData);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonString);
        using (UnityWebRequest www = new UnityWebRequest($"{Constants.ServerURL}/users/signin", UnityWebRequest.kHttpVerbPOST))
        {
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log($"Error: {www.error}");
                yield break;
            }
            else
            {
                var cookie = www.GetResponseHeader("set-cookie");
                if (!string.IsNullOrEmpty(cookie))
                {
                    int lastIndex = cookie.IndexOf('='); // 첫 번째 '='의 위치 찾기
                    if (lastIndex != -1 && lastIndex + 1 < cookie.Length) // '='이 존재하고, 뒤에 값이 있을 경우
                    {
                        string sid = cookie.Substring(lastIndex + 1).Split(';')[0]; // '=' 다음부터 세미콜론 이전까지 추출
                        Debug.Log("Session ID: " + sid);
                        PlayerPrefs.SetString("sid", sid);
                    }
                    else
                    {
                        // 올바른 쿠키 형식이 아님
                    }
                }
                else
                {
                    Debug.Log("쿠키가 비어 있음");
                }


                var resultString = www.downloadHandler.text;
                var signInResult = JsonUtility.FromJson<SignInResult>(resultString);
                if (signInResult.result == 0)
                {
                    GameManager.Instance.OpenConfirmPanel("유저네임 혹은 비밀번호가 유효하지 않습니다.", () => { init?.Invoke(); });
                }
                else if (signInResult.result == 1)
                {
                    GameManager.Instance.OpenConfirmPanel("유저네임 혹은 비밀번호가 유효하지 않습니다.", () => { init?.Invoke(); });
                }
                else if (signInResult.result == 2)
                {
                    GameManager.Instance.OpenConfirmPanel("로그인에 성공하였습니다", () => success?.Invoke());
                }
            }
        }
    }
}
