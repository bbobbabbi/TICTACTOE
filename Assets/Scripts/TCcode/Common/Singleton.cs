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
        //�� ��ȯ�� ȣ��Ǵ� �׼� �޼��� �Ҵ�
        // ���⼭ �޼ҵ� ������ �ϸ� ���� ������ ��ü�� �ϴ� ���� ����Ƿ�
        // Destory�� �������� �ٷ��������� ���ϰ� ����� �ѹ� �� �����Ǿ� 
        // OnSceneLoaded�� ȣ��ȴ� �׸��� ��������
        // SceneManager.sceneLoaded += OnSceneLoaded;
    }
    protected abstract void OnSceneLoaded(Scene scene, LoadSceneMode mode);
    

}
