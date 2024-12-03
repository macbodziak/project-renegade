using System;
using System.Collections;
using System.Collections.Generic;
using Navigation;
using UnityEngine;

public class PlayerActionManager : MonoBehaviour, IActionManager
{
    [SerializeField]
    private Unit _selectedUnit;
    private IAction _selectedAction;
    private static PlayerActionManager _instance;
    public ActionArgs actionArgs;
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
    public event EventHandler<SelectedUnitChangedEventArgs> UnitSelectionChangedEvent;
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
        actionArgs = new();
    }

    public void SetSelectedUnit(Unit unit)
    {
        if (unit == _selectedUnit)
        {
            return;
        }

        SelectionIndicator indicator;
        Unit previousUnit = _selectedUnit;
        //disable indicator of previously selected unit
        DeselectUnit();

        SelectUnit();

        //TODO - desing proper action system
        _selectedAction = new MoveAction();

        UnitSelectionChangedEvent?.Invoke(this, new SelectedUnitChangedEventArgs(previousUnit, _selectedUnit));

        void DeselectUnit()
        {
            if (_selectedUnit != null)
            {
                indicator = _selectedUnit.GetComponent<SelectionIndicator>();
                if (indicator != null)
                {
                    indicator.IsActive = false;
                }
            }
        }

        void SelectUnit()
        {
            _selectedUnit = unit;
            indicator = _selectedUnit.GetComponent<SelectionIndicator>();
            if (indicator != null)
            {
                indicator.IsActive = true;
            }

            DebugPrintAbilites(_selectedUnit.Abilities);
            InputManager.Instance.SetState(InputManager.State.SelectMovementTarget);
        }


        void DebugPrintAbilites(List<Ability> abilities)
        {
            foreach (var ab in abilities)
            {
                Debug.Log(ab.name);
            }
        }
    }


    public void CancelSelection()
    {
        //TODO - also set selected action to null
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
        UnitSelectionChangedEvent?.Invoke(this, new SelectedUnitChangedEventArgs(_selectedUnit, null));
    }


    //TODO remove this?
    // public void SetSelectedAction(IAction action)
    // {
    //     _selectedAction = action;
    //     // InputManager.Instance.SetState()
    // }

    public void SetSelectedAbility(Ability ability)
    {
        _selectedAction = ability.GetAction();
        InputManager.Instance.SetState(ability.InputState);
    }

    public void ExecuteSelectedAction()
    {
        if (_selectedAction == null)
        {
            return;
        }

        InputManager.Instance.SetState(InputManager.State.InputBlocked);
        LevelManager.Instance.NullifyPlayerWalkableAreas();
        ActionExecutionStartedEvent?.Invoke(this, EventArgs.Empty);
        _selectedAction.Execute(this, actionArgs);

    }


    public void OnSelectedAcionCompleted()
    {
        InputManager.Instance.SetState(InputManager.State.SelectMovementTarget);
        ActionExecutionFinishedEvent?.Invoke(this, EventArgs.Empty);
    }
}
