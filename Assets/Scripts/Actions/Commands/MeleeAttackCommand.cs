using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Utilities;

public class MeleeAttackCommand : ICommand
{
    private readonly Unit _attackingUnit;

    private Unit _targetUnit;

    public MeleeAttackCommand(Unit attackingUnit, Unit targetUnit)
    {
        _attackingUnit = attackingUnit;
        _targetUnit = targetUnit;
    }

    public async Task Execute(CancellationToken token)
    {
        Debug.Log("<color=#ffa08b>TO DO</color> add  logic here, such as HP reduction, AP reduction etc.");
        _attackingUnit.AnimationHandler?.TriggerMeleeAttackAnimation();
        while (_attackingUnit.AnimationHandler && _attackingUnit.AnimationHandler.CommandInProgress)
        {
            await Awaitable.NextFrameAsync(token);
        }
    }
}