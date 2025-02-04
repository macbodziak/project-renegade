using System;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class CommandStateBehaviour : StateMachineBehaviour
{
    [SerializeField] private List<TimedAnimationEvent> _events;

    private List<bool> _hasEventTriggered;

    UnitAnimationHandler _animationHandler;

    private void OnEnable()
    {
        Debug.Log("CommandStateBehaviour.OnEnable");
        _hasEventTriggered = new List<bool>(_events.Count);
        for (int i = 0; i < _events.Count; i++)
        {
            _hasEventTriggered.Add(false);
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float currentTime = stateInfo.normalizedTime;

        // foreach (TimedAnimationEvent evt in _events
        for (int i = 0; i < _events.Count; i++)
        {
            if (!_hasEventTriggered[i] && currentTime >= _events[i].TriggerTime)
            {
                _hasEventTriggered[i] = true;
                RaiseEvent(_events[i].Name);
            }
        }
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_animationHandler)
        {
            _animationHandler = animator.GetComponentInParent<UnitAnimationHandler>();
        }

        for (int i = 0; i < _hasEventTriggered.Count; i++)
        {
            _hasEventTriggered[i] = false;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        EventManager.InvokeCommandAnimationCompleted();
    }


    private void RaiseEvent(string eventName)
    {
        EventManager.InvokeOnCommandAnimationEvent(eventName);
    }
}