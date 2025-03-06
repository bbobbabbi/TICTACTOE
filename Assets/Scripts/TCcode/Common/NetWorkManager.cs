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


        // IDisposable �ֵ��� ���� ���� �ֵ� �� ������ ��Ȯ�ϰ� ������� �ٸ� �ֵ��� ������ �� �ִ� www.Dispose();
        // c#������ using�� ����� �̸� ������ �� �ִ�

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
                    GameManager.Instance.OpenConfirmPanel("�̹� �����ϴ� ������Դϴ�", () => init?.Invoke());
                    Debug.Log("�ߺ������");
                }
            }
            else
            {
                var result = www.downloadHandler.text;
                Debug.Log($"Result: {result}");
                 GameManager.Instance.OpenConfirmPanel("ȸ�������� �Ϸ�Ǿ����ϴ�", () => success?.Invoke());
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
                    Debug.Log("�α����� �ʿ��մϴ�.");
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
                    Debug.Log("�α����� �ʿ��մϴ�.");
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
                    Debug.Log("����ڸ� ã�� �� �����ϴ�.");
                }
                if (www.responseCode == 403)
                {
                    Debug.Log("�α����� �ʿ��մϴ�.");
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
                    int lastIndex = cookie.IndexOf('='); // ù ��° '='�� ��ġ ã��
                    if (lastIndex != -1 && lastIndex + 1 < cookie.Length) // '='�� �����ϰ�, �ڿ� ���� ���� ���
                    {
                        string sid = cookie.Substring(lastIndex + 1).Split(';')[0]; // '=' �������� �����ݷ� �������� ����
                        Debug.Log("Session ID: " + sid);
                        PlayerPrefs.SetString("sid", sid);
                    }
                    else
                    {
                        // �ùٸ� ��Ű ������ �ƴ�
                    }
                }
                else
                {
                    Debug.Log("��Ű�� ��� ����");
                }


                var resultString = www.downloadHandler.text;
                var signInResult = JsonUtility.FromJson<SignInResult>(resultString);
                if (signInResult.result == 0)
                {
                    GameManager.Instance.OpenConfirmPanel("�������� Ȥ�� ��й�ȣ�� ��ȿ���� �ʽ��ϴ�.", () => { init?.Invoke(); });
                }
                else if (signInResult.result == 1)
                {
                    GameManager.Instance.OpenConfirmPanel("�������� Ȥ�� ��й�ȣ�� ��ȿ���� �ʽ��ϴ�.", () => { init?.Invoke(); });
                }
                else if (signInResult.result == 2)
                {
                    GameManager.Instance.OpenConfirmPanel("�α��ο� �����Ͽ����ϴ�", () => success?.Invoke());
                }
            }
        }
    }
}
