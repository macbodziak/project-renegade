using System;
using System.Collections;
using System.Collections.Generic;
using Navigation;
using UnityEngine;

public class PlayerActionManager : PersistentSingelton<PlayerActionManager>, IActionManager
{
    [SerializeField]
    private Unit _selectedUnit;
    [SerializeField]
    private Ability _selectedAbility;

    public Unit SelectedUnit { get => _selectedUnit; }
    public Ability SelectedAbility { get => _selectedAbility; }
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


    public event Action<SelectedUnitChangedEventArgs> UnitSelectionChangedEvent;
    public event Action<SelectedAbilityChangedEventArgs> SelectedAbilityChangedEvent;
    public event Action ActionExecutionStartedEvent;
    public event Action ActionExecutionFinishedEvent;

    /// <summary>
    /// Initializes the PlayerActionManager and sets the selected unit to null upon awakening.
    /// </summary>
    protected override void InitializeOnAwake()
    {
        _selectedUnit = null;
    }


    /// <summary>
    /// Selects a unit, updates the selection indicator, raises the selection change event, 
    /// and sets the default ability to the unit's movement ability if a unit is selected.
    /// </summary>
    /// <param name="unit">The unit to select. Pass null to deselect the current unit.</param>
    public void SelectUnit(Unit unit)
    {
        if (unit == _selectedUnit) return;

        SelectionIndicator indicator;
        Unit previousUnit = null;

        //deselect previous unit if one was selected
        if (_selectedUnit != null)
        {
            previousUnit = _selectedUnit;
            indicator = _selectedUnit.GetComponent<SelectionIndicator>();
            if (indicator != null)
            {
                indicator.IsActive = false;
            }
            _selectedUnit = null;
        }

        _selectedUnit = unit;

        if (_selectedUnit == null)
        {
            //unselect ability
            SetSelectedAbility(null);
        }
        else
        {
            //activate the selection indicator
            indicator = _selectedUnit.GetComponent<SelectionIndicator>();
            if (indicator != null)
            {
                indicator.IsActive = true;
            }

            //set the default ability to movement
            SetSelectedAbility(_selectedUnit.MoveAbility);
        }

        //raise event 
        UnitSelectionChangedEvent?.Invoke(new SelectedUnitChangedEventArgs(previousUnit, _selectedUnit));
    }


    /// <summary>
    /// Updates the currently selected ability and adjusts the input state accordingly.
    /// </summary>
    /// <param name="ability">The ability to set as the currently selected ability. Pass null to deselect the ability.</param>
    public void SetSelectedAbility(Ability ability)
    {
        if (ability != null)
        {
            InputManager.Instance.SetState(ability.InputState);
        }
        else
        {
            if (_selectedUnit == null)
            {
                InputManager.Instance.SetState(InputManager.State.SelectUnit);
            }
            else
            {
                InputManager.Instance.SetState(InputManager.State.SelectMovementTarget);
            }
        }

        if (_selectedAbility == ability) return;

        Ability previousAbility = _selectedAbility;

        _selectedAbility = ability;

        SelectedAbilityChangedEvent?.Invoke(new SelectedAbilityChangedEventArgs(previousAbility, _selectedAbility));
    }


    /// <summary>
    /// Executes the currently selected action with the provided ability arguments.
    /// Blocks user input and triggers the action execution events.
    /// </summary>
    /// <param name="abilityArgs">Arguments required for the selected ability to execute.</param>

    public void ExecuteSelectedAction(AbilityArgs abilityArgs)
    {
        if (_selectedAbility == null)
        {
            return;
        }

        InputManager.Instance.SetState(InputManager.State.InputBlocked);
        ActionExecutionStartedEvent?.Invoke();
        _selectedAbility.Execute(this, abilityArgs);

    }


    /// <summary>
    /// Finalizes the execution of the selected action by resetting the default ability to movement, 
    /// setting the input state, clearing walkable areas, and triggering the action completion event.
    /// </summary>
    public void OnSelectedAcionCompleted()
    {
        if (_selectedUnit != null)
        {
            SetSelectedAbility(_selectedUnit.MoveAbility);
        }
        else
        {
            SetSelectedAbility(null);
        }
        LevelManager.Instance.NullifyPlayerWalkableAreas();
        ActionExecutionFinishedEvent?.Invoke();
    }

    /// <summary>
    /// Handles the cancellation of the current selection.
    /// If an ability is currently selected, it deselects.
    /// Otherwise, it deselects the currently selected unit.
    /// </summary>
    public void OnCancelSelection()
    {
        if (_selectedAbility != null && _selectedAbility != _selectedUnit.MoveAbility)
        {
            SetSelectedAbility(_selectedUnit.MoveAbility);
            return;
        }

        SelectUnit(null);
    }
}
