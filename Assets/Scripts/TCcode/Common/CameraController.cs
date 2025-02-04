using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [SerializeField] private float widthUnit = 6f;
    private Camera _camera;
    private void Start()
    {
        _camera = GetComponent<Camera>();
        //가로가 6 유닛이 나오도록 세로 크기를 구하고 카메라 사이즈는 세로의 절반이니 2를 나눠줌
        _camera.orthographicSize = widthUnit / _camera.aspect / 2;
    }
}
