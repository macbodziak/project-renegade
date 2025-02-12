using Navigation;
using UnityEngine;

public class SelectMovementTargetHandler : InputStateHandler
{
    int _hoveredNodeId;
    Path _path;
    WalkableArea _area;

    public override string PromptText => "select movement target";

    public SelectMovementTargetHandler(LayerMask unitLayerMask) : base(unitLayerMask)
    {
    }

    public override void OnEnter()
    {
        _path = null;
        _area = PlayerActionManager.Instance.SelectedUnit.GetWalkableArea();
        LevelManager.Instance.ShowWalkableArea(_area);
    }

    public override void OnExit()
    {
        LevelManager.Instance.HidePathPreview();
        _area = null;
    }

    public override void HandleInput()
    {
        if (IsMouseOverUI() == false)
        {
            UpdateRaycastHit();
            ScanObjectUnderCursor();


            if (_selectInputAction.WasPerformedThisFrame())
            {
                OnMouseClicked();
            }

            if (_selectFocusInputAction.WasPerformedThisFrame())
            {
                OnMouseDoubleClicked();
            }
        }

        if (_cancelInputAction.WasPerformedThisFrame())
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

        // NavGrid grid = gameObject.GetComponent<NavGrid>();
        // if (grid != null)
        // {
        //     OnMouseEnterGrid(grid);
        // }
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
            PlayerActionManager.Instance.SelectUnit(unit);
            LevelManager.Instance.HidePathPreview();
            _area = unit.GetWalkableArea();
            LevelManager.Instance.ShowWalkableArea(_area);
            _path = null;
        }
    }

    private void OnCancel()
    {
        PlayerActionManager.Instance.OnCancelSelection();
    }

    private void OnGridClicked(NavGrid grid)
    {
        int nodeId = grid.IndexAt(_currentHit.point);

        if (_area != null && _area.ContainsNode(nodeId))
        {
            LevelManager.Instance.HidePathPreview();

            MovementArgs args = new MovementArgs(PlayerActionManager.Instance.SelectedUnit, _path);
            PlayerActionManager.Instance.ExecuteSelectedAction(args);
        }
    }


    private void OnMouseExitGrid(NavGrid grid)
    {
        LevelManager.Instance.HidePathPreview();
    }

    private void OnMouseStayOverGrid(NavGrid grid)
    {
        int currentNodeId = grid.IndexAt(_currentHit.point);

        if (_hoveredNodeId != currentNodeId)
        {

            if (_area != null && _area.ContainsNode(currentNodeId))
            {
                _path = _area.GetPathFromNodeIndex(currentNodeId);
                LevelManager.Instance.ShowPathPreview(_path);
            }
            else
            {
                LevelManager.Instance.HidePathPreview();
            }
        }
        _hoveredNodeId = currentNodeId;
    }
}
