using UnityEngine;

public abstract class InputStateHandler
{
    protected LayerMask _layerMask;
    protected RaycastHit _currentHit;
    protected RaycastHit _previousHit;
    protected GameObject _previouslyHitObject;
    protected GameObject _currentlyHitObject;


    protected InputStateHandler(LayerMask unitLayerMask)
    {
        _layerMask = unitLayerMask;
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