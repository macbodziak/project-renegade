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
        //todo abstract the animator
        _attackingUnit.Animator.SetTrigger("AttackMelee");
        await Awaitable.WaitForSecondsAsync(0.7f, token);
    }
}