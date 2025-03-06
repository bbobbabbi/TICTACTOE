using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject Prefab;
    [SerializeField] private int poolSize;
    [SerializeField] private RectTransform parentsTransform;

    private Queue<GameObject> _pool;
    private static ObjectPool _instance;
    public static ObjectPool Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            _pool = new Queue<GameObject>();

            for (int i = 0; i < poolSize; i++)
            {
                CreateNewObject();
            }
        }
    }

    private void CreateNewObject()
    {
        GameObject newObject = Instantiate(Prefab);
        //ó�� ������ �� position�� �������� �ʰ� �θ� ���� ���� ���� ���ڰ��� false�� ��
        newObject.transform.SetParent(parentsTransform, false);
        newObject.SetActive(false);
        _pool.Enqueue(newObject);
    }

    public GameObject GetObject()
    {
        if (_pool.Count == 0) { CreateNewObject(); }
        GameObject dequeObject = _pool.Dequeue();
        dequeObject.SetActive(true);
        return dequeObject;
    }

    public void ReturnObject(GameObject returnObject)
    {
        returnObject.SetActive(false);
        _pool.Enqueue(returnObject);
    }
}
