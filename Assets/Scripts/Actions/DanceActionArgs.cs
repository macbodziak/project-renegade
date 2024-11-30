using UnityEngine;

public class DanceActionArgs : IActionArgs
{
    private Unit _unit;
    private float _duration;


    public Unit unit { get => _unit; }
    public float duration { get => _duration; }


    public DanceActionArgs(Unit unit, float duration)
    {
        _unit = unit;
        _duration = duration;
    }
}
