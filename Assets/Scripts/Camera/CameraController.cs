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
        Vector3 mouseTranslation = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            mouseTranslation += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            mouseTranslation += Vector3.back;
        }
        if (Input.GetKey(KeyCode.A))
        {
            mouseTranslation += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            mouseTranslation += Vector3.right;
        }

        mouseTranslation *= _speed * Time.deltaTime;

        MoveCamera(mouseTranslation);
    }

    private void MoveCamera(Vector3 vector)
    {
        _camera.transform.Translate(vector, Space.World);
    }
}




