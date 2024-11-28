using Navigation;
using UnityEngine;

public class SelectMovementTargetHandler : InputStateHandler
{
    public SelectMovementTargetHandler(LayerMask unitLayerMask) : base(unitLayerMask)
    {
    }

    public override void HandleInput()
    {
        UpdateRaycastHit();
        ScanObjectUnderCursor();


        if (selectAction.WasPerformedThisFrame())
        {
            Debug.Log("CLICK!!! at frame " + Time.frameCount);
            OnMouseClicked();
        }

        if (cancelAction.WasPerformedThisFrame())
        {
            OnCancel();
        }

        if (selectFocusAction.WasPerformedThisFrame())
        {
            OnMouseDoubleClicked();
        }
    }


    private void OnMouseClicked()
    {
        if (_currentlyHitObject == null)
        {
            return;
        }

        Unit clickedUnit = _currentlyHitObject.GetComponent<Unit>();
        if (clickedUnit != null)
        {
            if (clickedUnit.IsPlayer)
            {
                OnPlayerUnitClicked(clickedUnit);
            }
        }

        NavGrid grid = _currentlyHitObject.GetComponent<NavGrid>();
        if (grid != null)
        {
            OnGridClicked(grid);
        }

    }

    private void OnMouseDoubleClicked()
    {
        if (_currentlyHitObject == null)
        {
            return;
        }

        Unit clickedUnit = _currentlyHitObject.GetComponent<Unit>();
        if (clickedUnit != null)
        {
            if (clickedUnit.IsPlayer)
            {
                LevelManager.Instance.CamController.Teleport(clickedUnit.transform.position);
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

        NavGrid grid = gameObject.GetComponent<NavGrid>();
        if (grid != null)
        {
            OnMouseEnterGrid(grid);
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

        NavGrid grid = gameObject.GetComponent<NavGrid>();
        if (grid != null)
        {
            OnMouseExitGrid(grid);
        }
    }

    protected override void OnMouseStayOverObject(GameObject gameObject)
    {
        NavGrid grid = gameObject.GetComponent<NavGrid>();
        if (grid != null)
        {
            OnMouseStayOverGrid(grid);
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

    private void OnGridClicked(NavGrid grid)
    {
        int nodeId = grid.IndexAt(_currentHit.point);
        Vector2 gridCoord = grid.GridCoordinatesAt(_currentHit.point);
        Vector3 worldPosition = _currentHit.point;
        Debug.Log($"clicked Grid at world position:<color=<color=#c78bff> > {worldPosition} , -> {gridCoord}</color>");
        Actor actor = UnitSelectionManager.Instance.SelectedUnit.gameObject.GetComponent<Actor>();
        if (actor != null)
        {
            Path path = Pathfinder.FindPath(LevelManager.Instance.Grid, actor.NodeIndex, nodeId);
            LevelManager.Instance.ShowPathPreview(path);
        }
    }

    private void OnMouseEnterGrid(NavGrid grid)
    {
        int nodeId = grid.IndexAt(_currentHit.point);
        Vector2 gridCoord = grid.GridCoordinatesAt(_currentHit.point);
        Vector3 worldPosition = _currentHit.point;
        Debug.Log($"mouse entered Grid at world position:<color=orange> {worldPosition} , -> {gridCoord}</color>");
    }

    private void OnMouseExitGrid(NavGrid grid)
    {
        Debug.Log($"EXITING Grid at world position:");
    }

    private void OnMouseStayOverGrid(NavGrid grid)
    {

    }
}
