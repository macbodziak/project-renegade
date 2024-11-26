using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float _edgeMargin;
    [SerializeField] float _speed;
    Camera _camera;
    void Start()
    {
        _camera = FindAnyObjectByType<Camera>();
        Debug.Assert(_camera != null);
    }


    void Update()
    {

        Vector3 mousePosition = Input.mousePosition;
        Vector3 translation = Vector3.zero;

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Detect edges
        if (mousePosition.x <= _edgeMargin)
            translation += Vector3.left * _speed * Time.deltaTime;
        else if (mousePosition.x >= screenWidth - _edgeMargin)
            translation += Vector3.right * _speed * Time.deltaTime;

        if (mousePosition.y <= _edgeMargin)
            translation += Vector3.back * _speed * Time.deltaTime;
        else if (mousePosition.y >= screenHeight - _edgeMargin)
            translation += Vector3.forward * _speed * Time.deltaTime;

        MoveCamera(translation);
    }

    private void MoveCamera(Vector3 vector)
    {
        _camera.transform.Translate(vector, Space.World);
    }
}




