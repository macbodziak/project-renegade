using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

// <summary>
// Base class for Highlighters
// </summary>
public abstract class Highlighter : MonoBehaviour, IHighlighter
{
    protected HighlightState _state;

    public abstract HighlightState State { get; set; }
}
