using UnityEngine;
using Navigation;
using System.Threading.Tasks;


[CreateAssetMenu(fileName = "MoveAbility", menuName = "Abilitites/Move")]
public class MoveAbility : Ability
{

    public override InputManager.State InputState => InputManager.State.SelectMovementTarget;


    public async override void Execute(IActionManager actionManager, AbilityArgs abilityArgs)
    {
        MovementArgs args = abilityArgs as MovementArgs;
        Unit unit = args.Unit;
        Path path = args.Path;

        Task moveAlongTask = unit.MoveAlongPath(path);

        unit.animator.SetBool("Running", true);

        await moveAlongTask;

        unit.animator.SetBool("Running", false);

        actionManager.OnSelectedAcionCompleted();
    }
}
