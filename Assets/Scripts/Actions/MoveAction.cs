using Navigation;
using UnityEngine;
using System.Collections;
using System.Threading.Tasks;

public class MoveAction : IAction
{
    Unit _unit;
    Path _path;

    bool _inProgress;
    IActionManager _actionManager;

    public async void Execute(IActionManager actionManager, IActionArgs actionArgs)
    {
        MoveActionArgs args = actionArgs as MoveActionArgs;
        _unit = args.unit;
        _path = args.path;
        _actionManager = actionManager;

        _unit.MoveAlongPath(_path);
        _inProgress = true;

        if (_unit.actor.State == ActorState.Moving)
        {
            _unit.animator.SetBool("Running", true);
        }

        while (_unit.actor.State != ActorState.Idle)
        {
            await Task.Yield();
        }
        _inProgress = false;
        _unit.animator.SetBool("Running", false);
        _actionManager.OnSelectedAcionCompleted();
    }

    public bool InProgress()
    {
        return _inProgress;
    }
}
