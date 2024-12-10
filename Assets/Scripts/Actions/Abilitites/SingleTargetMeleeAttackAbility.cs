
using System.Threading.Tasks;
using UnityEngine;
using System;
using System.Collections.Generic;
using Utilities;

[CreateAssetMenu(fileName = "SingleTargetMeleeAttackAbility", menuName = "Abilitites/Single Target Melee Attack")]
public class SingleTargetMeleeAttackAbility : Ability
{
    public override InputManager.State InputState => InputManager.State.SelectSingleMeleeTarget;


    public override List<ICommand> GetCommands(AbilityArgs args)
    {
        SingleMeleeTargetArgs abArgs = args as SingleMeleeTargetArgs;
        List<ICommand> commands = new();
        commands.Add(new MoveAlongPathCommand(abArgs.AttackingUnit, abArgs.Path));
        commands.Add(new FaceTowardsCommand(abArgs.AttackingUnit, abArgs.TargetUnit.WorldPosition));
        commands.Add(new MeleeAttackCommand(abArgs.AttackingUnit, abArgs.TargetUnit));
        return commands;
    }
}