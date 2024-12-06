using System.Collections.Generic;
using Navigation;
using UnityEngine;

public class SelectSingleMeleeTargetHandler : InputStateHandler
{
    WalkableArea _walkablearea;
    Dictionary<Unit, Path> _pathCache;

    public override string PromptText => "select target";

    public SelectSingleMeleeTargetHandler(LayerMask layerMask) : base(layerMask)
    {
        _pathCache = new();
    }

    public override void OnEnter()
    {
        _walkablearea = PlayerActionManager.Instance.SelectedUnit.GetWalkableArea();
    }

    public override void OnExit()
    {
        LevelManager.Instance.HidePathPreview();
        _pathCache.Clear();
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
            if (!clickedUnit.IsPlayer)
            {
                OnEnemyUnitClicked(clickedUnit);
            }
        }
    }

    private void OnEnemyUnitClicked(Unit clickedUnit)
    {
        int attackerIndex = PlayerActionManager.Instance.SelectedUnit.NodeIndex;
        int targetIndex = clickedUnit.NodeIndex;

        if (LevelManager.Instance.Grid.AreAdjacent(attackerIndex, targetIndex))
        {
            //Target next to attacker
            PlayerActionManager.Instance.ExecuteSelectedAction(new SingleMeleeTargetArgs(PlayerActionManager.Instance.SelectedUnit, clickedUnit, null));
        }
        else if (_walkablearea != null && _walkablearea.IsNodeAdjacent(targetIndex))
        {
            //target within area reach
            Path path = Pathfinder.FindPath(LevelManager.Instance.Grid, attackerIndex, targetIndex, true);
            PlayerActionManager.Instance.ExecuteSelectedAction(new SingleMeleeTargetArgs(PlayerActionManager.Instance.SelectedUnit, clickedUnit, path));
        }
        else
        {
            // target out of reach
            Debug.Log($"<color=#ffef8b> target out of reach</color>");
            //todo play some audio que??
        }

    }


    protected override void OnMouseEnterObject(GameObject gameObject)
    {
        Unit unit = gameObject.GetComponent<Unit>();
        if (unit != null)
        {
            if (!unit.IsPlayer)
            {
                SelectionIndicator indicator = unit.GetComponent<SelectionIndicator>();
                indicator.IsReviewed = true;

                //if unit is within the reachable area show path
                if (_walkablearea != null && _walkablearea.IsNodeAdjacent(unit.NodeIndex))
                {
                    Path path;
                    //check if path was already calculated this iteration
                    if (_pathCache.ContainsKey(unit))
                    {
                        path = _pathCache[unit];
                    }
                    else
                    {
                        //if path not yet cached, calculate it and cahce it
                        path = Pathfinder.FindPath(LevelManager.Instance.Grid,
                                                   PlayerActionManager.Instance.SelectedUnit.NodeIndex,
                                                   unit.NodeIndex,
                                                   true);
                        _pathCache.Add(unit, path);
                    }

                    LevelManager.Instance.ShowPathPreview(path);
                }
            }
        }
    }

    protected override void OnMouseExitObject(GameObject gameObject)
    {
        Unit unit = gameObject.GetComponent<Unit>();
        if (unit != null && !unit.IsPlayer)
        {
            SelectionIndicator indicator = unit.GetComponent<SelectionIndicator>();
            indicator.IsReviewed = false;

            LevelManager.Instance.HidePathPreview();
        }
    }

    private void OnCancel()
    {
        PlayerActionManager.Instance.SelectUnit(null);
    }

}
