using UnityEngine;
using System;
using System.Collections.Generic;

public class SelectUnitHandler : InputStateHandler
{
    public SelectUnitHandler(LayerMask unitLayerMask) : base(unitLayerMask)
    {
    }

    public override void HandleInput()
    {
        UpdateRaycastHit();
        ScanObjectUnderCursor();

        if (Input.GetMouseButtonDown(0))
        {
            OnMouseClicked();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnCancel();
        }
    }


    private void OnMouseClicked()
    {
        if (_currentlyHitObject != null)
        {
            Unit clickedUnit = _currentHit.collider.gameObject.GetComponent<Unit>();
            if (clickedUnit != null)
            {
                if (clickedUnit.IsPlayer)
                {
                    OnPlayerUnitClicked(clickedUnit);
                }
            }
        }
    }

    protected override void OnMouseEnterObject(GameObject gameObject)
    {
        Unit unit = gameObject.GetComponent<Unit>();

        if (unit != null && unit.IsPlayer)
        {
            SelectionIndicator indicator = unit.GetComponent<SelectionIndicator>();
            indicator.IsReviewed = true;
        }
    }

    protected override void OnMouseExitObject(GameObject gameObject)
    {
        Unit unit = gameObject.GetComponent<Unit>();
        if (unit != null && unit.IsPlayer)
        {
            SelectionIndicator indicator = unit.GetComponent<SelectionIndicator>();
            indicator.IsReviewed = false;
        }
    }

    private void OnPlayerUnitClicked(Unit unit)
    {
        UnitSelectionManager.Instance.SetSelectedUnit(unit);
    }

    private void OnCancel()
    {
        UnitSelectionManager.Instance.CancelSelection();
    }
}
