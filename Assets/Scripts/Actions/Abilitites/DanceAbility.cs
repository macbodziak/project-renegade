using System.Collections.Generic;
using UnityEngine;
using Utilities;

[CreateAssetMenu(fileName = "DanceAbility", menuName = "Abilities/Dance")]
public class DanceAbility : Ability
{
    [SerializeField] float _duration = 2.5f;

    public float Duration { get => _duration; }

    public override InputManager.State InputState => InputManager.State.Confirm;

    public override List<ICommand> GetCommands(AbilityArgs args)
    {
        ConfirmArgs abArgs = args as ConfirmArgs;
        List<ICommand> commands = new();
        commands.Add(new DanceCommand(abArgs.Unit, _duration));
        return commands;
    }
}