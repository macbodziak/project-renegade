using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Utilities;

public class MeleeAttackCommand : ICommand
{
    private readonly Unit _attackingUnit;

    private Unit _targetUnit;

    private bool _inProgress = false;

    public MeleeAttackCommand(Unit attackingUnit, Unit targetUnit)
    {
        _attackingUnit = attackingUnit;
        _targetUnit = targetUnit;
    }

    public async Task Execute(CancellationToken token)
    {
        Debug.Log("<color=#ffa08b>TO DO</color> add  logic here, such as HP reduction, AP reduction etc.");
        _inProgress = true;
        _attackingUnit.AnimationHandler?.TriggerMeleeAttackAnimation();
        EventManager.Instance.OnCommandAnimationCompleted += OnAnimationCompleted;
        EventManager.Instance.OnCommandAnimationEvent += OnAnimationEvent;
        while (_inProgress && !token.IsCancellationRequested)
        {
            await Awaitable.NextFrameAsync(token);
        }

        CleanUp();
    }


    private void OnAnimationCompleted()
    {
        _inProgress = false;
    }

    private void OnAnimationEvent(string eventName)
    {
        Debug.Log($"Event: <<color=#FD7272>{eventName}</color>> has been triggered");
    }

    private void CleanUp()
    {
        EventManager.Instance.OnCommandAnimationCompleted -= OnAnimationCompleted;
        EventManager.Instance.OnCommandAnimationEvent -= OnAnimationEvent;
    }
}