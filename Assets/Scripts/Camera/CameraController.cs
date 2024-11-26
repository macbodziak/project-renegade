using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] float _edgeMargin;
    [SerializeField] float _speed;
    Camera _camera;
    InputAction moveCameraAction;
    void Start()
    {
        _camera = FindAnyObjectByType<Camera>();
        Debug.Assert(_camera != null);
        moveCameraAction = InputSystem.actions.FindAction("MoveCamera");
    }


    void Update()
    {
        Vector2 moveCameraInput = moveCameraAction.ReadValue<Vector2>();

        Vector3 mouseTranslation = new Vector3(moveCameraInput.x, 0f, moveCameraInput.y) * _speed * Time.deltaTime;

        MoveCamera(mouseTranslation);
    }

    private void MoveCamera(Vector3 vector)
    {
        _camera.transform.Translate(vector, Space.World);
    }
}




