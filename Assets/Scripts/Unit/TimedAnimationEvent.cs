using System;
using UnityEngine;

[Serializable]
public struct TimedAnimationEvent
{
    public string Name;

    [Range(0f, 1f)] public float TriggerTime;
}