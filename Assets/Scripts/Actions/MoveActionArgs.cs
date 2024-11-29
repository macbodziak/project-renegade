using UnityEngine;
using Navigation;

public class MoveActionArgs : IActionArgs
{
    private Unit _unit;
    private Path _path;


    public Unit unit { get => _unit; set => _unit = value; }
    public Path path { get => _path; set => _path = value; }


    public MoveActionArgs(Unit unit, Path path)
    {
        _unit = unit;
        _path = path;
    }
}
