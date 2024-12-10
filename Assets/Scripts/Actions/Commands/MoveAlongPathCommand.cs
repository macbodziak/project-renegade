using Utilities;
using Navigation;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;

public class MoveAlongPathCommand : ICommand
{
    private Unit _unit;
    private Path _path;

    public MoveAlongPathCommand(Unit unit, Path path)
    {
        _unit = unit;
        _path = path;
    }

    public Task Execute(CancellationToken token)
    {
        return _unit.MoveAlongPath(_path, token);
    }
}
