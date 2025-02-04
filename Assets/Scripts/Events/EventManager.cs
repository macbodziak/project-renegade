using System;
using UnityEngine;

public class EventManager : PersistentSingelton<EventManager>
{
    public event Action<string> OnCommandAnimationEvent;
    public event Action OnCommandAnimationCompleted;

    public static void InvokeOnCommandAnimationEvent(string command)
    {
        Instance.OnCommandAnimationEvent?.Invoke(command);
    }

    public static void InvokeCommandAnimationCompleted()
    {
        Instance.OnCommandAnimationCompleted?.Invoke();
    }

    protected override void InitializeOnAwake()
    {
    }
}