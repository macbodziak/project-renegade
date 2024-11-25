using UnityEngine;
using System;
using UnityEditor;
using System.Collections.Generic;

public class InputManager : MonoBehaviour
{
    [SerializeField] LayerMask _unitLayerMask;
    private static InputManager _instance;
    public event EventHandler InputStateChangedEvent;
    private InputHandler _currentInputHandler;
    private InputState _currentState;
    private InputState _nextState;
    private bool _isStateChangeRequested;

    public InputState CurrentState
    {
        get { return _currentState; }
    }

    // this array of input hanlders must be the same as the order of enums in InputState
    // there has to be an input state class created for derived from BaseInputState
    private List<InputHandler> inputHandlers;

    public static InputManager Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            InitializeInputHanlders();
            InitializationStateOnAwake();
        }
    }

    private void Update()
    {
        _currentInputHandler.HandleInput();

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
        _currentInputHandler.OnExit();
        _currentState = _nextState;
        _currentInputHandler = inputHandlers[(int)_nextState];
        _nextState = InputState.None;
        _currentInputHandler.OnEnter();
        _isStateChangeRequested = false;
        InputStateChangedEvent?.Invoke(this, EventArgs.Empty);
    }

    //
    // Summary:
    // Set the state to be transitioned to. The actual transition happen in TransitionInputState()
    // at the end of Update(), after finishing handling input form previous state
    // This method should be called from outside this class to request a state change
    public void SetState(InputState state)
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
            new SelectUnitAndActionHandler(_unitLayerMask)
        };
    }


    private void InitializationStateOnAwake()
    {
        //state related variables
        _isStateChangeRequested = false;
        _currentState = InputState.SelectUnitAndAction;
        _nextState = InputState.None;
        _currentInputHandler = inputHandlers[(int)_currentState];
    }
}

