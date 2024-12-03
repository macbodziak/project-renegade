using UnityEngine;
using UnityEngine.InputSystem;

public class SelectUnitHandler : InputStateHandler
{
    public SelectUnitHandler(LayerMask unitLayerMask) : base(unitLayerMask)
    {
    }

    public override void OnEnter()
    {
        LevelManager.Instance.HideWalkableArea();
    }

    public override void HandleInput()
    {
        if (IsMouseOverUI() == false)
        {
            UpdateRaycastHit();
            ScanObjectUnderCursor();

            if (selectAction.WasPerformedThisFrame())
            {
                OnMouseClicked();
            }
        }

        if (cancelAction.WasPerformedThisFrame())
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

                if (selectFocusAction.WasPerformedThisFrame())
                {
                    LevelManager.Instance.CamController.Teleport(clickedUnit.transform.position);
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
        PlayerActionManager.Instance.SetSelectedUnit(unit);
    }

    private void OnCancel()
    {
        PlayerActionManager.Instance.CancelSelection();
    }
}
