using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // 씬이 로드될 때마다 호출됩니다
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // 구독 해제
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {if (scene.name == "Game")
        GameManager.Instance.InitGame();
    }
}
