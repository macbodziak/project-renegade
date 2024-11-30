using UnityEngine;

public class DanceAction : IAction
{

    Unit _unit;
    bool _inProgress;
    IActionManager _actionManager;
    float _duration;

    public async void Execute(IActionManager actionManager, IActionArgs actionArgs)
    {
        DanceActionArgs args = actionArgs as DanceActionArgs;

        _actionManager = actionManager;
        _inProgress = true;
        _unit = args.unit;
        _duration = args.duration;

        _unit.animator.SetBool("Dancing", true);
        await Awaitable.WaitForSecondsAsync(_duration);
        _unit.animator.SetBool("Dancing", false);
        _actionManager.OnSelectedAcionCompleted();
    }

    public bool InProgress()
    {
        return _inProgress;
    }
}