using UnityEngine;
using Navigation;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Utilities;


[CreateAssetMenu(fileName = "MoveAbility", menuName = "Abilitites/Move")]
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


    // public async override void Execute(IActionManager actionManager, AbilityArgs abilityArgs)
    // {
    //     MovementArgs args = abilityArgs as MovementArgs;
    //     Unit unit = args.Unit;
    //     Path path = args.Path;

    //     Task moveAlongTask = unit.MoveAlongPath(path);

    //     unit.animator.SetBool("Running", true);
    //     try
    //     {
    //         await moveAlongTask;
    //         unit.animator.SetBool("Running", false);
    //     }
    //     catch (OperationCanceledException e)
    //     {
    //         Debug.Log($"{nameof(OperationCanceledException)} thrown with message: {e.Message}");
    //     }
    //     finally
    //     {
    //         actionManager.OnSelectedAbilityExecutionCompleted();
    //     }
}

