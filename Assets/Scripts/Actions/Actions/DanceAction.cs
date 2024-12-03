using Sirenix.OdinInspector;
using UnityEngine;


public class DanceAction : IAction
{
    private Unit _unit;
    float _duration = 2.25f;
    bool _inProgress;

    public async void Execute(IActionManager actionManager, ActionArgs actionArgs)
    {
        _unit = actionArgs.ActingUnit;

        _inProgress = true;
        _unit.animator.SetBool("Dancing", true);
        await Awaitable.WaitForSecondsAsync(_duration);
        _unit.animator.SetBool("Dancing", false);
        _inProgress = false;
        actionManager.OnSelectedAcionCompleted();
    }

    public bool InProgress()
    {
        return _inProgress;
    }
}