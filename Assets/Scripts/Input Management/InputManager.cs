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
        Confirm,
        InputBlocked,
        None
    }

    [SerializeField]
    [Tooltip("Layer Mask used by Input State Handlers that need to detect both units and grid")]
    LayerMask _inputLayerMask;
    public event Action InputStateChangedEvent;
    private InputStateHandler _currentInputStateHandler;
    private State _currentState;
    private State _nextState;
    private bool _isStateChangeRequested;

    public State CurrentState
    {
        get { return _currentState; }
    }

    // this array of input hanlders must be the same as the order of enums in InputState
    // there has to be an input state class created for derived from BaseInputState
    private List<InputStateHandler> inputHandlers;

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
        _currentInputStateHandler = inputHandlers[(int)_nextState];
        _nextState = State.None;
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


    private void InitializeInputHanlders()
    {
        inputHandlers = new()
        {
            new SelectUnitHandler(_inputLayerMask),
            new SelectMovementTargetHandler(_inputLayerMask),
            new ConfirmHandler(0),
            new InputBlockedHandler(0)
        };
    }


    private void InitializationStateOnAwake()
    {
        //state related variables
        _isStateChangeRequested = false;
        _currentState = State.SelectUnit;
        _nextState = State.None;
        _currentInputStateHandler = inputHandlers[(int)_currentState];
    }

#if UNITY_EDITOR
    void OnGUI()
    {
        GUI.Label(new Rect(25, 25, 200, 30), $"{_currentInputStateHandler}");
    }

#endif
}

