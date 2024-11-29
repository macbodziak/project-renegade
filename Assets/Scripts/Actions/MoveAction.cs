using Navigation;
using UnityEngine;
using System.Collections;

public class MoveAction : IAction
{
    Unit _unit;
    Path _path;

    bool _inProgress;
    IActionManager _actionManager;

    public void Execute(IActionManager actionManager, IActionArgs actionArgs)
    {
        MoveActionArgs args = actionArgs as MoveActionArgs;
        _unit = args.unit;
        _path = args.path;
        _actionManager = actionManager;

        _unit.MoveAlongPath(_path);
        if (_unit.actor.State == ActorState.Moving)
        {
            _inProgress = true;
            _actionManager.StartCoroutine(ExecutionCoroutine());
        }
        else
        {
            _actionManager.OnSelectedAcionCompleted();
        }

    }

    public bool InProgress()
    {
        return _inProgress;
    }

    private IEnumerator ExecutionCoroutine()
    {
        while (_unit.actor.State != ActorState.Idle)
        {
            yield return null;
        }
        _inProgress = false;
        _actionManager.OnSelectedAcionCompleted();
    }
}
