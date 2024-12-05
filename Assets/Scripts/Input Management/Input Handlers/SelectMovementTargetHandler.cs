using Navigation;
using UnityEngine;

public class SelectMovementTargetHandler : InputStateHandler
{
    int _previousNodeId;
    Path _path;

    public override string PromptText => "select movement target";

    public SelectMovementTargetHandler(LayerMask unitLayerMask) : base(unitLayerMask)
    {
    }

    public override void OnEnter()
    {
        _previousNodeId = -1;
        _path = null;
        WalkableArea area = PlayerActionManager.Instance.SelectedUnit.GetWalkableArea();
        LevelManager.Instance.ShowWalkableArea(PlayerActionManager.Instance.SelectedUnit.GetWalkableArea());
    }

    public override void OnExit()
    {
        LevelManager.Instance.HidePathPreview();
    }

    public override void HandleInput()
    {
        if (IsMouseOverUI() == false)
        {
            UpdateRaycastHit();
            ScanObjectUnderCursor();


            if (_selectAction.WasPerformedThisFrame())
            {
                OnMouseClicked();
            }

            if (_selectFocusAction.WasPerformedThisFrame())
            {
                OnMouseDoubleClicked();
            }
        }

        if (_cancelAction.WasPerformedThisFrame())
        {
            OnCancel();
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
        if (unit != PlayerActionManager.Instance.SelectedUnit)
        {
            PlayerActionManager.Instance.SetSelectedUnit(unit);
            LevelManager.Instance.HidePathPreview();
            LevelManager.Instance.ShowWalkableArea(PlayerActionManager.Instance.SelectedUnit.GetWalkableArea());
            _path = null;
        }
    }

    private void OnCancel()
    {
        PlayerActionManager.Instance.CancelSelection();
    }

    private void OnGridClicked(NavGrid grid)
    {
        int nodeId = grid.IndexAt(_currentHit.point);

        WalkableArea area = PlayerActionManager.Instance.SelectedUnit.GetWalkableArea();

        if (area != null && area.ContainsNode(nodeId))
        {
            if (_previousNodeId == nodeId)
            {
                LevelManager.Instance.HidePathPreview();

                MovementArgs args = new MovementArgs(PlayerActionManager.Instance.SelectedUnit, _path);
                PlayerActionManager.Instance.ExecuteSelectedAction(args);

            }
            else
            {
                Actor actor = PlayerActionManager.Instance.SelectedUnit.gameObject.GetComponent<Actor>();
                if (actor != null)
                {
                    _path = Pathfinder.FindPath(LevelManager.Instance.Grid, actor.NodeIndex, nodeId);
                    LevelManager.Instance.ShowPathPreview(_path);
                    _previousNodeId = nodeId;
                }
            }
        }

    }

    private void OnMouseEnterGrid(NavGrid grid)
    {
        int nodeId = grid.IndexAt(_currentHit.point);
        Vector2 gridCoord = grid.GridCoordinatesAt(_currentHit.point);
        Vector3 worldPosition = _currentHit.point;
    }

    private void OnMouseExitGrid(NavGrid grid)
    {
    }

    private void OnMouseStayOverGrid(NavGrid grid)
    {
    }
}
