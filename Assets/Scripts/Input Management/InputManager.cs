using UnityEngine;
using System;
using UnityEditor;
using System.Collections.Generic;

public class InputManager : PersistentSingelton<InputManager>
{
    public enum State
    {
        SelectUnit,
        SelectMovementTarget,
        SelectSingleMeleeTarget,
        Confirm,
        InputBlocked,
        Null
    }

    [SerializeField]
    [Tooltip("Layer Mask used by Input State Handlers that need to detect both units and grid")]
    LayerMask _inputLayerMask;
    private InputStateHandler _currentInputStateHandler;
    private State _currentState;
    private State _nextState;
    private bool _isStateChangeRequested;


    public State CurrentState
    {
        get { return _currentState; }
    }


    public event Action InputStateChangedEvent;

    // for each input state, there needs to be a coresponding input stte handler
    private Dictionary<State, InputStateHandler> _inputHandlers;

    protected override void InitializeOnAwake()
    {
        InitializeInputHanlders();
        InitializationStateOnAwake();
    }


    private void Update()
    {
        _currentInputStateHandler.HandleInput();

        if (_isStateChangeRequested)
        {
            TransitionInputState();
        }
    }

    //
    // Summary:
    // This funtion handles the transition, but setting the state to be transitioned is done 
    // via SetState(State state).
    // should not be called form outside of this class
    private void TransitionInputState()
    {
        _currentInputStateHandler.OnExit();
        _currentState = _nextState;
        _currentInputStateHandler = _inputHandlers[_nextState];
        _nextState = State.Null;
        _currentInputStateHandler.OnEnter();
        _isStateChangeRequested = false;
        InputStateChangedEvent?.Invoke();
    }

    //
    // Summary:
    // Set the state to be transitioned to. The actual transition happen in TransitionInputState()
    // at the end of Update(), after finishing handling input form previous state
    // This method should be called from outside this class to request a state change
    public void SetState(State state)
    {
        if (state == _currentState)
        {
            return;
        }
        _nextState = state;
        _isStateChangeRequested = true;
    }


    public string GetInputPromptText()
    {
        return _currentInputStateHandler.PromptText;
    }


    private void InitializeInputHanlders()
    {
        _inputHandlers = new();
        _inputHandlers.Add(State.SelectUnit, new SelectUnitHandler(_inputLayerMask));
        _inputHandlers.Add(State.SelectMovementTarget, new SelectMovementTargetHandler(_inputLayerMask));
        _inputHandlers.Add(State.SelectSingleMeleeTarget, new SelectSingleMeleeTargetHandler(_inputLayerMask));
        _inputHandlers.Add(State.InputBlocked, new InputBlockedHandler(0));
        _inputHandlers.Add(State.Confirm, new ConfirmHandler(0));
        _inputHandlers.Add(State.Null, new NullHandler(0));
    }


    private void InitializationStateOnAwake()
    {
        //state related variables
        _isStateChangeRequested = false;
        _currentState = State.SelectUnit;
        _nextState = State.Null;
        _currentInputStateHandler = _inputHandlers[_currentState];
    }

#if UNITY_EDITOR
    void OnGUI()
    {
        GUI.Label(new Rect(25, 25, 200, 30), $"{_currentInputStateHandler}");
    }

#endif
}

