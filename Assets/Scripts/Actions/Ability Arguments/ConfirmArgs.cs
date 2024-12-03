using UnityEngine;

public class ConfirmArgs : AbilityArgs
{
    Unit _unit;

    public ConfirmArgs(Unit actingUnit)
    {
        _unit = actingUnit;
    }

    public Unit Unit { get => _unit; }
}
