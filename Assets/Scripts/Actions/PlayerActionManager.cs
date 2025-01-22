using System;
using System.Collections.Generic;
using Navigation;
using Sirenix.OdinInspector;
using Utilities;

public class PlayerActionManager : PersistentSingelton<PlayerActionManager>, IActionManager
{
    private CommandQueue _abilityCommandQueue;

    public Unit SelectedUnit { get; private set; }
    public Ability SelectedAbility { get; private set; }

    public int SelectedUnitNodeIndex
    {
        get
        {
            if (SelectedUnit == null)
            {
                return -1;
            }
            return SelectedUnit.GetComponent<Actor>().NodeIndex;
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
        SelectedUnit = null;
        _abilityCommandQueue = new CommandQueue();
        _abilityCommandQueue.OnExecutionCompleted += OnSelectedAbilityExecutionCompleted;
    }


    /// <summary>
    /// Selects a unit, updates the selection indicator, raises the selection change event, 
    /// and sets the default ability to the unit's movement ability if a unit is selected.
    /// </summary>
    /// <param name="unit">The unit to select. Pass null to deselect the current unit.</param>
    public void SelectUnit(Unit unit)
    {
        if (unit == SelectedUnit) return;

        SelectionIndicator indicator;
        Unit previousUnit = null;

        //deselect previous unit if one was selected
        if (SelectedUnit is not null)
        {
            previousUnit = SelectedUnit;
            indicator = SelectedUnit.SelectionIndicator;
            if (indicator is not null)
            {
                indicator.IsActive = false;
            }
            SelectedUnit = null;
        }

        SelectedUnit = unit;

        if (SelectedUnit is null)
        {
            //unselect ability
            SetSelectedAbility(null);
        }
        else
        {
            //activate the selection indicator
            indicator = SelectedUnit.SelectionIndicator;
            if (indicator is not null)
            {
                indicator.IsActive = true;
            }

            //set the default ability to movement
            SetSelectedAbility(SelectedUnit.MoveAbility);
        }

        //raise event 
        UnitSelectionChangedEvent?.Invoke(new SelectedUnitChangedEventArgs(previousUnit, SelectedUnit));
    }


    /// <summary>
    /// Updates the currently selected ability and adjusts the input state accordingly.
    /// </summary>
    /// <param name="ability">The ability to set as the currently selected ability. Pass null to deselect the ability.</param>
    public void SetSelectedAbility(Ability ability)
    {
        if (ability is not null)
        {
            InputManager.Instance.SetState(ability.InputState);
        }
        else
        {
            if (SelectedUnit is null)
            {
                InputManager.Instance.SetState(InputManager.State.SelectUnit);
            }
            else
            {
                InputManager.Instance.SetState(InputManager.State.SelectMovementTarget);
            }
        }

        if (SelectedAbility == ability) return;

        Ability previousAbility = SelectedAbility;

        SelectedAbility = ability;

        SelectedAbilityChangedEvent?.Invoke(new SelectedAbilityChangedEventArgs(previousAbility, SelectedAbility));
    }


    /// <summary>
    /// Executes the currently selected ability with the provided ability arguments.
    /// Blocks user input until all commands given by ability are executed.  
    /// </summary>
    /// <param name="abilityArgs">Arguments required for the selected ability to execute.</param>
    public void ExecuteSelectedAction(AbilityArgs abilityArgs)
    {
        if (SelectedAbility is null)
        {
            return;
        }

        InputManager.Instance.SetState(InputManager.State.InputBlocked);
        ActionExecutionStartedEvent?.Invoke();
        List<ICommand> commands = SelectedAbility.GetCommands(abilityArgs);
        _abilityCommandQueue.Add(commands);
        _abilityCommandQueue.Execute();
    }

    /// <summary>
    /// Finalizes the execution of the selected action by resetting the default ability to movement, 
    /// setting the input state, clearing walkable areas, and triggering the action completion event.
    /// </summary>
    public void OnSelectedAbilityExecutionCompleted()
    {
        if (SelectedUnit is not null)
        {
            SetSelectedAbility(SelectedUnit.MoveAbility);
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
        if (SelectedAbility is not null && SelectedAbility != SelectedUnit.MoveAbility)
        {
            SetSelectedAbility(SelectedUnit.MoveAbility);
            return;
        }

        SelectUnit(null);
    }

    [Button("Stop Command Queue")]
    public void DebugStopActions()
    {
        _abilityCommandQueue.Stop();
    }
}
