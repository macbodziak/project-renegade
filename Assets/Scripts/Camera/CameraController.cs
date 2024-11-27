using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] float _linearSpeed = 1;
    [SerializeField] float _rotationalSpeed = 30;
    InputAction translateCameraAction;
    InputAction rotateCameraAction;
    InputAction zoomCameraAction;

    void Start()
    {
        translateCameraAction = InputSystem.actions.FindAction("MoveCamera");
        rotateCameraAction = InputSystem.actions.FindAction("RotateCamera");

#if DEBUG
        Debug.Assert(translateCameraAction != null);
        Debug.Assert(rotateCameraAction != null);
#endif
    }


    void Update()
    {
        Vector2 linearDelta = translateCameraAction.ReadValue<Vector2>();
        float rotationalDelta = rotateCameraAction.ReadValue<float>();

        Vector3 mouseTranslation = new Vector3(linearDelta.x, 0f, linearDelta.y) * _linearSpeed * Time.deltaTime;

        gameObject.transform.Translate(mouseTranslation, Space.Self);
        gameObject.transform.Rotate(0f, rotationalDelta * _rotationalSpeed * Time.deltaTime, 0f);
    }
}




