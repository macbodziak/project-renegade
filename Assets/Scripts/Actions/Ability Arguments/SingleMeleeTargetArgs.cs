using UnityEngine;
using Navigation;

public class SingleMeleeTargetArgs : AbilityArgs
{
    private Unit _attackingUnit;
    private Unit _targetUnit;
    private Path _path;

    public SingleMeleeTargetArgs(Unit attackingUnit, Unit targetUnit, Path path)
    {
        _attackingUnit = attackingUnit;
        _targetUnit = targetUnit;
        _path = path;
    }

    public Path Path { get => _path; }
    public Unit AttackingUnit { get => _attackingUnit; }
    public Unit TargetUnit { get => _targetUnit; }
}
