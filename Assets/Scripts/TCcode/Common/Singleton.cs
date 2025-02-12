using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;
    public static T Instance { 
        get {
            if (_instance == null) { 
                _instance = FindObjectOfType<T>();
                if (_instance == null) {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name; 
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else 
        { 
            Destroy(gameObject);
        }
        //씬 전환시 호출되는 액션 메서드 할당
        // 여기서 메소드 구독을 하면 새로 생성된 객체가 하는 일이 생기므로
        // Destory를 만났더라도 바로지워지지 못하고 요놈이 한번 더 구독되어 
        // OnSceneLoaded가 호출된다 그리고 지워지고
        // SceneManager.sceneLoaded += OnSceneLoaded;
    }
    protected abstract void OnSceneLoaded(Scene scene, LoadSceneMode mode);
    

}
