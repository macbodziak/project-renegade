using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] float _linearSpeed = 1;
    [SerializeField] float _rotationalSpeed = 30;
    [SerializeField] Bounds _bounds;
    InputAction translateCameraAction;
    InputAction rotateCameraAction;

    void Start()
    {
        translateCameraAction = InputSystem.actions.FindAction("MoveCamera");
        rotateCameraAction = InputSystem.actions.FindAction("RotateCamera");

        _bounds = LevelManager.Instance.Grid.WorldBounds;

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
        gameObject.transform.position = ClampVector3ToBounds(gameObject.transform.position, _bounds);

        gameObject.transform.Rotate(0f, rotationalDelta * _rotationalSpeed * Time.deltaTime, 0f);
    }


    private Vector3 ClampVector3ToBounds(Vector3 point, Bounds bounds)
    {
        Vector3 min = bounds.min;
        Vector3 max = bounds.max;

        return new Vector3(
            Mathf.Clamp(point.x, min.x, max.x),
            Mathf.Clamp(point.y, min.y, max.y),
            Mathf.Clamp(point.z, min.z, max.z)
        );
    }

}




