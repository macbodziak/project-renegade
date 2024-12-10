using System.Collections.Generic;
using UnityEngine;
using Utilities;

[CreateAssetMenu(fileName = "DanceAbility", menuName = "Abilitites/Dance")]
public class DanceAbility : Ability
{
    [SerializeField] float _duration = 2.5f;

    public float Duration { get => _duration; }

    public override InputManager.State InputState => InputManager.State.Confirm;

    // public async override void Execute(IActionManager actionManager, AbilityArgs abilityArgs)
    // {
    //     ConfirmArgs args = abilityArgs as ConfirmArgs;
    //     Unit unit = args.Unit;

    //     unit.animator.SetBool("Dancing", true);
    //     await Awaitable.WaitForSecondsAsync(_duration);
    //     unit.animator.SetBool("Dancing", false);
    //     actionManager.OnSelectedAbilityExecutionCompleted();
    // }

    public override List<ICommand> GetCommands(AbilityArgs args)
    {
        ConfirmArgs abArgs = args as ConfirmArgs;
        List<ICommand> commands = new();
        commands.Add(new DanceCommand(abArgs.Unit, _duration));
        return commands;
    }
}
