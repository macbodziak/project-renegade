using System;
using System.Collections;
using Navigation;
using UnityEngine;

public class PlayerActionManager : MonoBehaviour, IActionManager
{
    [SerializeField]
    private Unit _selectedUnit;
    private IAction _selectedAction;
    private static PlayerActionManager _instance;
    public static PlayerActionManager Instance { get { return _instance; } }
    public Unit SelectedUnit { get => _selectedUnit; }
    public int SelectedUnitNodeIndex
    {
        get
        {
            if (_selectedUnit == null)
            {
                return -1;
            }
            return _selectedUnit.GetComponent<Actor>().NodeIndex;
        }
    }
    public IAction SelectedAction { get => _selectedAction; }
    public event EventHandler UnitSelectionChangedEvent;
    public event EventHandler UnitSelectionCanceledEvent;
    public event EventHandler ActionExecutionStartedEvent;
    public event EventHandler ActionExecutionFinishedEvent;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            InitializationStateOnAwake();
        }
    }

    private void InitializationStateOnAwake()
    {
        _selectedUnit = null;
    }

    public void SetSelectedUnit(Unit unit)
    {
        if (unit == _selectedUnit)
        {
            return;
        }

        SelectionIndicator indicator;

        //disable indicator of previously selected unit
        if (_selectedUnit != null)
        {
            indicator = _selectedUnit.GetComponent<SelectionIndicator>();
            if (indicator != null)
            {
                indicator.IsActive = false;
            }
        }

        _selectedUnit = unit;
        indicator = _selectedUnit.GetComponent<SelectionIndicator>();
        if (indicator != null)
        {
            indicator.IsActive = true;
        }

        InputManager.Instance.SetState(InputManager.State.SelectMovementTarget);
        //TODO - desing proper action system
        _selectedAction = new MoveAction();

        UnitSelectionChangedEvent?.Invoke(this, EventArgs.Empty);
    }


    public void CancelSelection()
    {
        if (_selectedUnit != null)
        {
            SelectionIndicator indicator = _selectedUnit.GetComponent<SelectionIndicator>();
            if (indicator != null)
            {
                indicator.IsActive = false;
            }
            InputManager.Instance.SetState(InputManager.State.SelectUnit);
        }
        _selectedUnit = null;
        UnitSelectionCanceledEvent?.Invoke(this, EventArgs.Empty);
    }


    //TODO remove this?
    public void SetSelectedAction(IAction action)
    {
        _selectedAction = action;
    }

    public void ExecuteSelectedAction(IActionArgs args)
    {
        if (_selectedAction == null)
        {
            return;
        }

        InputManager.Instance.SetState(InputManager.State.InputBlocked);
        LevelManager.Instance.NullifyPlayerWalkableAreas();
        ActionExecutionStartedEvent?.Invoke(this, EventArgs.Empty);
        _selectedAction.Execute(this, args);

    }


    public void OnSelectedAcionCompleted()
    {
        InputManager.Instance.SetState(InputManager.State.SelectMovementTarget);
        ActionExecutionFinishedEvent?.Invoke(this, EventArgs.Empty);
    }
}
