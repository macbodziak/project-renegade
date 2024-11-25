using UnityEngine;
using System;
using System.Collections.Generic;

public class SelectUnitAndActionHandler : InputHandler
{

    // List<Unit>
    public SelectUnitAndActionHandler(LayerMask unitLayerMask) : base(unitLayerMask)
    {
    }

    public override void HandleInput()
    {
        GameObject hitObject = RaycastToGameObject();

        SetObjectUnderCursor(hitObject);
        if (Input.GetMouseButtonDown(0))
        {
            OnMouseClicked();
        }
    }


    private void OnMouseClicked()
    {
        if (_objectUnderCursor != null)
        {
            Unit clickedUnit = _objectUnderCursor.GetComponent<Unit>();
            if (clickedUnit != null)
            {
                if (clickedUnit.IsPlayer)
                {
                    UnitSelectionManager.Instance.SetSelectedUnit(clickedUnit);
                    // SelectionIndicator indicator = clickedUnit.GetComponent<SelectionIndicator>();
                    // indicator.IsActive = true;
                }
            }
        }
    }

    protected override void OnSwitchObjectUnderCursor(GameObject previous, GameObject current)
    {
        Unit unit = previous.GetComponent<Unit>();
        if (unit != null)
        {
            if (unit.IsPlayer)
            {
                SelectionIndicator indicator = unit.GetComponent<SelectionIndicator>();
                indicator.IsReviewed = false;
            }
        }

        unit = current.GetComponent<Unit>();
        if (unit != null)
        {
            if (unit.IsPlayer)
            {
                SelectionIndicator indicator = unit.GetComponent<SelectionIndicator>();
                indicator.IsReviewed = true;
            }
        }
    }

    protected override void OnMouseEnterObject(GameObject gameObject)
    {
        Unit unit = gameObject.GetComponent<Unit>();
        if (unit != null)
        {
            if (unit.IsPlayer)
            {
                SelectionIndicator indicator = unit.GetComponent<SelectionIndicator>();
                indicator.IsReviewed = true;
            }
        }
    }

    protected override void OnMouseExitObject(GameObject gameObject)
    {
        Unit unit = gameObject.GetComponent<Unit>();
        if (unit.IsPlayer)
        {
            SelectionIndicator indicator = unit.GetComponent<SelectionIndicator>();
            indicator.IsReviewed = false;
        }
    }
}
