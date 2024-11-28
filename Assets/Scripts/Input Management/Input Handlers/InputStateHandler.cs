using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InputStateHandler
{
    protected LayerMask _layerMask;
    protected RaycastHit _currentHit;
    protected RaycastHit _previousHit;
    protected GameObject _previouslyHitObject;
    protected GameObject _currentlyHitObject;
    protected InputAction selectAction;
    protected InputAction selectFocusAction;
    protected InputAction cancelAction;

    protected InputStateHandler(LayerMask layerMask)
    {
        _layerMask = layerMask;

        selectAction = InputSystem.actions.FindAction("Select");
        selectFocusAction = InputSystem.actions.FindAction("SelectAndFocus");
        cancelAction = InputSystem.actions.FindAction("Cancel");

#if DEBUG
        Debug.Assert(selectAction != null);
        Debug.Assert(cancelAction != null);
        Debug.Assert(selectFocusAction != null);
#endif
    }


    protected void UpdateRaycastHit()
    {
        _previousHit = _currentHit;
        _previouslyHitObject = _currentlyHitObject;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out _currentHit, Mathf.Infinity, _layerMask);
        _currentlyHitObject = _currentHit.collider == null ? null : _currentHit.collider.gameObject;
    }

    public abstract void HandleInput();


    public virtual void OnEnter() {; }


    public virtual void OnExit() {; }

    protected void ScanObjectUnderCursor()
    {

        if (_currentlyHitObject != null && _currentlyHitObject == _previouslyHitObject)
        {
            OnMouseStayOverObject(_currentlyHitObject);
        }

        if (_currentlyHitObject != _previouslyHitObject)
        {
            if (_previouslyHitObject != null)
            {
                OnMouseExitObject(_previouslyHitObject);
            }
            if (_currentlyHitObject != null)
            {
                OnMouseEnterObject(_currentlyHitObject);
            }
        }
    }


    protected virtual void OnMouseEnterObject(GameObject gameObject)
    {
    }

    protected virtual void OnMouseExitObject(GameObject gameObject)
    {
    }

    protected virtual void OnMouseStayOverObject(GameObject gameObject)
    {
    }

}