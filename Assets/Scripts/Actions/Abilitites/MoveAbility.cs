using UnityEngine;
using System.Collections.Generic;
using Utilities;


[CreateAssetMenu(fileName = "MoveAbility", menuName = "Abilities/Move")]
public class MoveAbility : Ability
{
    public override InputManager.State InputState => InputManager.State.SelectMovementTarget;

    public override List<ICommand> GetCommands(AbilityArgs args)
    {
        MovementArgs movementArgs = args as MovementArgs;
        List<ICommand> commands = new();
        commands.Add(new MoveAlongPathCommand(movementArgs.Unit, movementArgs.Path));
        return commands;
    }
}