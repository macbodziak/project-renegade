using System;
using System.Collections;
using System.Collections.Generic;
using Navigation;
using UnityEngine;

public class PlayerActionManager : MonoBehaviour, IActionManager
{
    [SerializeField]
    private Unit _selectedUnit;
    private Ability _selectedAbility;
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
    public Ability SelectedAbility { get => _selectedAbility; }

    public event Action<SelectedUnitChangedEventArgs> UnitSelectionChangedEvent;
    public event Action ActionExecutionStartedEvent;
    public event Action ActionExecutionFinishedEvent;


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
        Unit previousUnit = _selectedUnit;

        //disable indicator of previously selected unit
        DeselectUnit();

        SelectUnit();

        //when selecting a unit, always set the selected ability to movement, which is the default ability
        SetSelectedAbility(_selectedUnit.MoveAbility);

        UnitSelectionChangedEvent?.Invoke(new SelectedUnitChangedEventArgs(previousUnit, _selectedUnit));

        //local functions:
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
        }
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
        _selectedAbility = null;

        UnitSelectionChangedEvent?.Invoke(new SelectedUnitChangedEventArgs(_selectedUnit, null));
    }


    public void SetSelectedAbility(Ability ability)
    {
        _selectedAbility = ability;
        InputManager.Instance.SetState(ability.InputState);
    }

    public void ExecuteSelectedAction(AbilityArgs abilityArgs)
    {
        if (_selectedAbility == null)
        {
            return;
        }

        InputManager.Instance.SetState(InputManager.State.InputBlocked);
        LevelManager.Instance.NullifyPlayerWalkableAreas();
        ActionExecutionStartedEvent?.Invoke();
        _selectedAbility.Execute(this, abilityArgs);

    }


    public void OnSelectedAcionCompleted()
    {
        InputManager.Instance.SetState(InputManager.State.SelectMovementTarget);
        ActionExecutionFinishedEvent?.Invoke();
    }
}
