using Navigation;
using UnityEngine;
using Utilities;
public class MoveUnitAction
{
    Actor _actor;
    NavGrid _grid;
    Path _path;

    public MoveUnitAction(Actor actor, NavGrid grid, Path path)
    {
        _actor = actor;
        _grid = grid;
        _path = path;
    }

    public void Execute()
    {
        _actor.MoveAlongPath(_path);
    }

}
