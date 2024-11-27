using System;
using System.Collections;
using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField][MinValue(0.01f)] float _linearSpeed = 1;
    [SerializeField][MinValue(0.1f)] float _rotationalSpeed = 30;
    [SerializeField][MinValue(0.1f)] float _teleportSpeed = 2;
    Bounds _bounds;
    bool _busy;
    InputAction translateCameraAction;
    InputAction rotateCameraAction;
    // [SerializeField] CinemachineOrbitalFollow _orbitalFlow;
    [SerializeField][Required] CinemachineInputAxisController _inputAxisController;

    void Start()
    {
        _busy = false;

        translateCameraAction = InputSystem.actions.FindAction("MoveCamera");
        rotateCameraAction = InputSystem.actions.FindAction("RotateCamera");

        _bounds = LevelManager.Instance.Grid.WorldBounds;

#if DEBUG
        Debug.Assert(translateCameraAction != null);
        Debug.Assert(rotateCameraAction != null);
        // Debug.Assert(_orbitalFlow != null);
        Debug.Assert(_inputAxisController != null);
#endif
    }


    void Update()
    {
        if (_busy)
        {
            return;
        }

        Vector2 linearDelta = translateCameraAction.ReadValue<Vector2>();
        float rotationalDelta = rotateCameraAction.ReadValue<float>();

        Vector3 mouseTranslation = new Vector3(linearDelta.x, 0f, linearDelta.y) * _linearSpeed * Time.deltaTime;

        gameObject.transform.Translate(mouseTranslation, Space.Self);
        gameObject.transform.position = ClampVector3ToBounds(gameObject.transform.position, _bounds);

        gameObject.transform.Rotate(0f, rotationalDelta * _rotationalSpeed * Time.deltaTime, 0f);
    }

    public IEnumerator TeleportCoroutine(Vector3 targetPoint)
    {
        Vector3 startPoint = transform.position;
        float progress = 0f;
        _busy = true;
        _inputAxisController.enabled = false;

        while (progress < 1f)
        {
            progress += Time.deltaTime * _teleportSpeed;
            transform.position = Vector3.Lerp(startPoint, targetPoint, progress);
            yield return null;
        }
        _busy = false;
        _inputAxisController.enabled = true;
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

    public void Teleport(Vector3 targetPosition)
    {
        StartCoroutine(TeleportCoroutine(targetPosition));
    }

}




