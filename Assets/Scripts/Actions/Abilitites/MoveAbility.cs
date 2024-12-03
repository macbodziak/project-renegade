using UnityEngine;
using Navigation;


[CreateAssetMenu(fileName = "MoveAbility", menuName = "Abilitites/Move")]
public class MoveAbility : Ability
{

    public override InputManager.State InputState => InputManager.State.SelectMovementTarget;


    public async override void Execute(IActionManager actionManager, AbilityArgs abilityArgs)
    {
        MovementArgs args = abilityArgs as MovementArgs;
        Unit unit = args.Unit;
        Path path = args.Path;

        unit.MoveAlongPath(path);

        if (unit.actor.State == ActorState.Moving)
        {
            unit.animator.SetBool("Running", true);
        }

        while (unit.actor.State != ActorState.Idle)
        {
            await Awaitable.NextFrameAsync();
        }
        unit.animator.SetBool("Running", false);
        actionManager.OnSelectedAcionCompleted();
    }
}
