using UnityEngine;

public abstract class InputStateHandler
{
    protected LayerMask _unitLayerMask;
    protected GameObject _objectUnderCursor;

    protected InputStateHandler(LayerMask unitLayerMask)
    {
        _unitLayerMask = unitLayerMask;
    }

    protected GameObject RaycastToGameObject()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out hit, Mathf.Infinity, _unitLayerMask);

        if (hit.collider == null)
        {
            return null;
        }
        return hit.collider.gameObject;
    }

    public abstract void HandleInput();


    public virtual void OnEnter() {; }


    public virtual void OnExit() {; }

    protected void SetObjectUnderCursor(GameObject gameObject)
    {
        if (_objectUnderCursor == gameObject)
        {
            return;
        }

        if (gameObject == null && _objectUnderCursor != null)
        {
            OnMouseExitObject(_objectUnderCursor);
            _objectUnderCursor = null;
        }

        if (gameObject != null && _objectUnderCursor == null)
        {
            _objectUnderCursor = gameObject;
            OnMouseEnterObject(_objectUnderCursor);
        }

        if (gameObject != null && _objectUnderCursor != gameObject)
        {
            OnSwitchObjectUnderCursor(_objectUnderCursor, gameObject);
            _objectUnderCursor = gameObject;
        }
    }

    protected virtual void OnSwitchObjectUnderCursor(GameObject previous, GameObject current)
    {
    }

    protected virtual void OnMouseEnterObject(GameObject gameObject)
    {
    }

    protected virtual void OnMouseExitObject(GameObject gameObject)
    {
    }

}