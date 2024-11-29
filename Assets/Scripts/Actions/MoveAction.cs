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

        _actionManager.StartCoroutine(ExecutionCoroutine());
    }

    public bool InProgress()
    {
        return _inProgress;
    }

    private IEnumerator ExecutionCoroutine()
    {
        _unit.MoveAlongPath(_path);
        _inProgress = true;

        if (_unit.actor.State == ActorState.Moving)
        {
            _unit.animator.SetBool("Running", true);
        }

        while (_unit.actor.State != ActorState.Idle)
        {
            yield return null;
        }
        _inProgress = false;
        _unit.animator.SetBool("Running", false);
        _actionManager.OnSelectedAcionCompleted();
    }
}
