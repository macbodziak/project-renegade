using Navigation;
using UnityEngine;

public class MoveAction : IAction
{
    private Unit _unit;
    private Path _path;
    private bool _inProgress;

    public async void Execute(IActionManager actionManager, ActionArgs actionArgs)
    {
        _unit = actionArgs.ActingUnit;
        _path = actionArgs.Path;

        _inProgress = true;
        _unit.MoveAlongPath(_path);

        if (_unit.actor.State == ActorState.Moving)
        {
            _unit.animator.SetBool("Running", true);
        }

        while (_unit.actor.State != ActorState.Idle)
        {
            await Awaitable.NextFrameAsync();
        }
        _unit.animator.SetBool("Running", false);
        _inProgress = false;
        actionManager.OnSelectedAcionCompleted();
    }

    public bool InProgress()
    {
        return _inProgress;
    }
}
