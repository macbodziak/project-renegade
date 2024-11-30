using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

// <summary>
// Base class for Selection Indicators
// </summary>
public abstract class SelectionIndicator : MonoBehaviour
{
    private bool _isActive;
    private bool _isReviewed;
    private SelectionIndicatorState _state;
    protected SelectionIndicatorState State { get => _state; }

    public bool IsActive
    {
        get { return _isActive; }

        set
        {
            if (value)
            {
                _isActive = true;
            }
            else
            {
                _isActive = false;
            }
            UpdateState();
        }

    }

    public bool IsReviewed
    {
        get { return _isReviewed; }
        set
        {
            if (value)
            {
                _isReviewed = true;
            }
            else
            {
                _isReviewed = false;
            }
            UpdateState();
        }
    }

    protected void UpdateState()
    {
        if (_isReviewed == true)
        {
            _state = SelectionIndicatorState.Reviewed;
            OnEnterState();
            return;
        }

        if (_isActive == true && _isReviewed == false)
        {
            _state = SelectionIndicatorState.Selected;
            OnEnterState();
            return;
        }

        if (_isActive == false && _isReviewed == false)
        {
            _state = SelectionIndicatorState.InActive;
            OnEnterState();
            return;
        }
    }

    protected abstract void OnEnterState();

}
