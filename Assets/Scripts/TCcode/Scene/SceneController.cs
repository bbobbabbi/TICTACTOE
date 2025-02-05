using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // ���� �ε�� ������ ȣ��˴ϴ�
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // ���� ����
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {if (scene.name == "Game")
        GameManager.Instance.InitGame();
    }
}
