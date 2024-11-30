using UnityEngine;
using Navigation;

public class MoveActionArgs : IActionArgs
{
    private Unit _unit;
    private Path _path;


    public Unit unit { get => _unit; }
    public Path path { get => _path; }


    public MoveActionArgs(Unit unit, Path path)
    {
        _unit = unit;
        _path = path;
    }
}
