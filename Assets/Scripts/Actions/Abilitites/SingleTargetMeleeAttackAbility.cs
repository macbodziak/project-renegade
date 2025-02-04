using UnityEngine;
using System.Collections.Generic;
using Utilities;

[CreateAssetMenu(fileName = "SingleTargetMeleeAttackAbility", menuName = "Abilities/Single Target Melee Attack")]
public class SingleTargetMeleeAttackAbility : Ability
{
    public override InputManager.State InputState => InputManager.State.SelectSingleMeleeTarget;


    public override List<ICommand> GetCommands(AbilityArgs abilityArgs)
    {
        SingleMeleeTargetArgs args = abilityArgs as SingleMeleeTargetArgs;
        List<ICommand> commands = new();
        if (args.Path != null)
        {
            commands.Add(new MoveAlongPathCommand(args.AttackingUnit, args.Path));
        }

        commands.Add(new FaceTowardsCommand(args.AttackingUnit, args.TargetUnit.WorldPosition));
        commands.Add(new MeleeAttackCommand(args.AttackingUnit, args.TargetUnit));
        return commands;
    }
}