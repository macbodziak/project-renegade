using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

// <summary>
// Base class for Highlighters
// </summary>
public abstract class SelectionIndicator : MonoBehaviour, ISelectionIndicator
{
    protected SelectionIndicatorState _state;

    public abstract SelectionIndicatorState State { get; set; }
}
