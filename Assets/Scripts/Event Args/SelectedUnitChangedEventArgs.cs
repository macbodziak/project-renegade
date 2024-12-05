using UnityEngine;

public struct SelectedUnitChangedEventArgs
{
    Unit _previousUnit;
    Unit _currentUnit;

    public SelectedUnitChangedEventArgs(Unit previousUnit, Unit CurrentUnit)
    {
        _previousUnit = previousUnit;
        _currentUnit = CurrentUnit;
    }

    public Unit PreviousUnit { get => _previousUnit; }
    public Unit CurrentUnit { get => _currentUnit; }
}
