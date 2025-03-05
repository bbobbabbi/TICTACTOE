using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
public struct SignupData {
    public string username;
    public string nickname;
    public string password;
}
public class SignupPannelController : MonoBehaviour
{
    [SerializeField] private TMP_InputField _usernameInputField;
    [SerializeField] private TMP_InputField _nicknameInputField;
    [SerializeField] private TMP_InputField _passwordInputField;
    [SerializeField] private TMP_InputField _confirmPasswordInputField;
    public void OnClickConfirmButton()
    {
        var username = _usernameInputField.text;
        var nickname = _nicknameInputField.text;
        var password = _passwordInputField.text;
        var confirmPassword = _confirmPasswordInputField.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(nickname) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
        {
            GameManager.Instance.OpenConfirmPanel("��� �׸��� ä���ּ���", () => {
                InitTextField();
            });
            return;
        }

        if (password.Equals(confirmPassword))
        {
            SignupData signupData = new SignupData();
            signupData.username = username;
            signupData.nickname = nickname;
            signupData.password = password;
            // ������ SignupData �����ϸ鼭 ȸ������ ����
            StartCoroutine(NetWorkManager.Instance.Signup(signupData, InitTextField, () => Destroy(gameObject)));
        }
        else {
            GameManager.Instance.OpenConfirmPanel("��й�ȣ�� ���� �ʽ��ϴ�", () => {
                _passwordInputField.text = string.Empty;
                _confirmPasswordInputField.text = string.Empty;
            });
            return;
        }
    }

  
    public void OnClickCancelButton() {
        InitTextField();
    }

    private void InitTextField() {
        _usernameInputField.text = string.Empty;
        _nicknameInputField.text = string.Empty;
        _passwordInputField.text = string.Empty;
        _confirmPasswordInputField.text = string.Empty;
    }

    public void OnClickBackButton() {
        Destroy(gameObject);
    }
}
