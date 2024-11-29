using Navigation;
using Game;
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
            _unit.actor.MovementFinishedEvent += HandleMovementFinished;
        }

    }

    public bool InProgress()
    {
        return _inProgress;
    }

    // private IEnumerator MonitorProgressCoroutine()
    // {

    // }

    private void HandleMovementFinished(object sender, ActorFinishedMovementEventArgs eventArgs)
    {
        _inProgress = false;
        _actionManager.OnSelectedAcionCompleted();
        _unit.actor.MovementFinishedEvent -= HandleMovementFinished;
    }
}
